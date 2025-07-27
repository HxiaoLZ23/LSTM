using LSTM.Models;
using LSTM.Models.DTOs;

namespace LSTM.Business.Interfaces
{
    public interface ISentimentAnalysisBusinessService
    {
        Task<SentimentAnalysisResultDto> AnalyzeTextAsync(string text, int userId);
        Task<IEnumerable<SentimentAnalysisResultDto>> GetUserAnalysisHistoryAsync(int userId);
        Task<bool> SaveAnalysisResultAsync(SentimentAnalysisResultDto result, int userId);
        Task<IEnumerable<SentimentAnalysisResultDto>> BatchAnalyzeAsync(IEnumerable<string> texts, int userId);
        Task<bool> DeleteAnalysisAsync(int analysisId, int userId);
    }
} 