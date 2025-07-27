using LSTM.Models;
using LSTM.Models.DTOs;

namespace LSTM.ML.Interfaces
{
    public interface ISentimentAnalysisService
    {
        Task<SentimentAnalysisResultDto> AnalyzeSentimentAsync(string text);
        Task<bool> TrainModelAsync(int datasetId, string modelName);
        Task<bool> LoadModelAsync(int modelId);
        Task<double> EvaluateModelAsync(int modelId, int datasetId);
        Task<IEnumerable<SentimentAnalysisResultDto>> BatchAnalyzeAsync(IEnumerable<string> texts);
        Task<bool> IsModelLoadedAsync();
    }
} 