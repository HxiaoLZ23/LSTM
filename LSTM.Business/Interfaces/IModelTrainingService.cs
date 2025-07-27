using LSTM.Models;

namespace LSTM.Business.Interfaces
{
    public interface IModelTrainingService
    {
        Task<bool> CreateDatasetAsync(string name, string description, int userId);
        Task<bool> ImportTrainingDataAsync(int datasetId, string filePath);
        Task<bool> ImportTrainingDataFromTextAsync(int datasetId, IEnumerable<(string text, SentimentType sentiment)> data);
        Task<bool> TrainModelAsync(int datasetId, string modelName);
        Task<double> EvaluateModelAsync(int modelId);
        Task<IEnumerable<Dataset>> GetUserDatasetsAsync(int userId);
        Task<IEnumerable<MLModel>> GetUserModelsAsync(int userId);
        Task<IEnumerable<TrainingData>> GetDatasetTrainingDataAsync(int datasetId);
        Task<bool> UpdateTrainingDataAsync(TrainingData data);
        Task<bool> DeleteTrainingDataAsync(int trainingDataId);
        Task<bool> AddTrainingDataAsync(int datasetId, string text, SentimentType sentiment);
        Task<bool> DeleteDatasetAsync(int datasetId, int userId);
        Task<bool> DeleteModelAsync(int modelId, int userId);
        Task<bool> LoadModelAsync(int modelId);
        Task<bool> UpdateDatasetAsync(int datasetId, string newName, string? newDescription);
    }
} 