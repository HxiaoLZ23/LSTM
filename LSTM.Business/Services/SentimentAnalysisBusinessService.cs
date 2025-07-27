using LSTM.Business.Interfaces;
using LSTM.Data.Interfaces;
using LSTM.ML.Interfaces;
using LSTM.Models;
using LSTM.Models.DTOs;
using Microsoft.Extensions.Logging;

namespace LSTM.Business.Services
{
    public class SentimentAnalysisBusinessService : ISentimentAnalysisBusinessService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISentimentAnalysisService _sentimentAnalysisService;
        private readonly ILogger<SentimentAnalysisBusinessService> _logger;

        public SentimentAnalysisBusinessService(
            IUnitOfWork unitOfWork,
            ISentimentAnalysisService sentimentAnalysisService,
            ILogger<SentimentAnalysisBusinessService> logger)
        {
            _unitOfWork = unitOfWork;
            _sentimentAnalysisService = sentimentAnalysisService;
            _logger = logger;
        }

        public async Task<SentimentAnalysisResultDto> AnalyzeTextAsync(string text, int userId)
        {
            try
            {
                _logger.LogInformation("Starting sentiment analysis for user {UserId}", userId);

                // 执行情感分析
                var result = await _sentimentAnalysisService.AnalyzeSentimentAsync(text);

                // 保存分析结果
                await SaveAnalysisResultAsync(result, userId);

                _logger.LogInformation("Sentiment analysis completed for user {UserId}", userId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error analyzing sentiment for user {UserId}", userId);
                throw;
            }
        }

        public async Task<IEnumerable<SentimentAnalysisResultDto>> GetUserAnalysisHistoryAsync(int userId)
        {
            try
            {
                var analyses = await _unitOfWork.SentimentAnalyses.FindAsync(sa => sa.UserId == userId);
                
                return analyses.Select(sa => new SentimentAnalysisResultDto
                {
                    Text = sa.Text,
                    PredictedSentiment = sa.PredictedSentiment,
                    Confidence = sa.Confidence,
                    AnalyzedAt = sa.AnalyzedAt
                }).OrderByDescending(sa => sa.AnalyzedAt);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting analysis history for user {UserId}", userId);
                throw;
            }
        }

        public async Task<bool> SaveAnalysisResultAsync(SentimentAnalysisResultDto result, int userId)
        {
            try
            {
                var analysis = new SentimentAnalysis
                {
                    Text = result.Text,
                    PredictedSentiment = result.PredictedSentiment,
                    Confidence = result.Confidence,
                    AnalyzedAt = result.AnalyzedAt,
                    UserId = userId
                };

                await _unitOfWork.SentimentAnalyses.AddAsync(analysis);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Analysis result saved for user {UserId}", userId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving analysis result for user {UserId}", userId);
                return false;
            }
        }

        public async Task<IEnumerable<SentimentAnalysisResultDto>> BatchAnalyzeAsync(IEnumerable<string> texts, int userId)
        {
            try
            {
                _logger.LogInformation("Starting batch sentiment analysis for user {UserId}", userId);

                var results = new List<SentimentAnalysisResultDto>();

                foreach (var text in texts)
                {
                    var result = await _sentimentAnalysisService.AnalyzeSentimentAsync(text);
                    results.Add(result);
                    
                    // 保存每个分析结果
                    await SaveAnalysisResultAsync(result, userId);
                }

                _logger.LogInformation("Batch sentiment analysis completed for user {UserId}", userId);
                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in batch sentiment analysis for user {UserId}", userId);
                throw;
            }
        }

        public async Task<bool> DeleteAnalysisAsync(int analysisId, int userId)
        {
            try
            {
                var analysis = await _unitOfWork.SentimentAnalyses.FirstOrDefaultAsync(sa => sa.Id == analysisId && sa.UserId == userId);
                
                if (analysis == null)
                {
                    _logger.LogWarning("Analysis {AnalysisId} not found for user {UserId}", analysisId, userId);
                    return false;
                }

                _unitOfWork.SentimentAnalyses.Remove(analysis);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Analysis {AnalysisId} deleted for user {UserId}", analysisId, userId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting analysis {AnalysisId} for user {UserId}", analysisId, userId);
                return false;
            }
        }
    }
} 