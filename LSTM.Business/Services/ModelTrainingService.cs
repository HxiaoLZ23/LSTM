using LSTM.Business.Interfaces;
using LSTM.Data.Interfaces;
using LSTM.ML.Interfaces;
using LSTM.Models;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace LSTM.Business.Services
{
    public class ModelTrainingService : IModelTrainingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISentimentAnalysisService _sentimentAnalysisService;
        private readonly ILogger<ModelTrainingService> _logger;

        public ModelTrainingService(
            IUnitOfWork unitOfWork,
            ISentimentAnalysisService sentimentAnalysisService,
            ILogger<ModelTrainingService> logger)
        {
            _unitOfWork = unitOfWork;
            _sentimentAnalysisService = sentimentAnalysisService;
            _logger = logger;
        }

        public async Task<bool> CreateDatasetAsync(string name, string description, int userId)
        {
            try
            {
                var dataset = new Dataset
                {
                    Name = name,
                    Description = description,
                    UserId = userId,
                    CreatedAt = DateTime.Now,
                    IsActive = true
                };

                await _unitOfWork.Datasets.AddAsync(dataset);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Dataset {Name} created for user {UserId}", name, userId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating dataset {Name} for user {UserId}", name, userId);
                return false;
            }
        }

        public async Task<bool> ImportTrainingDataAsync(int datasetId, string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    _logger.LogWarning("Training data file not found: {FilePath}", filePath);
                    return false;
                }

                var dataset = await _unitOfWork.Datasets.GetByIdAsync(datasetId);
                if (dataset == null)
                {
                    _logger.LogWarning("Dataset {DatasetId} not found", datasetId);
                    return false;
                }

                var extension = Path.GetExtension(filePath).ToLower();
                List<(string text, SentimentType sentiment)> data = new();

                switch (extension)
                {
                    case ".csv":
                        data = await ReadCsvFileAsync(filePath);
                        break;
                    case ".json":
                        data = await ReadJsonFileAsync(filePath);
                        break;
                    case ".txt":
                        data = await ReadTextFileAsync(filePath);
                        break;
                    default:
                        _logger.LogWarning("Unsupported file format: {Extension}", extension);
                        return false;
                }

                return await ImportTrainingDataFromTextAsync(datasetId, data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error importing training data from file {FilePath}", filePath);
                return false;
            }
        }

        public async Task<bool> ImportTrainingDataFromTextAsync(int datasetId, IEnumerable<(string text, SentimentType sentiment)> data)
        {
            try
            {
                var trainingDataList = data.Select(d => new TrainingData
                {
                    Text = d.text,
                    ActualSentiment = d.sentiment,
                    DatasetId = datasetId,
                    CreatedAt = DateTime.Now
                }).ToList();

                await _unitOfWork.TrainingData.AddRangeAsync(trainingDataList);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Imported {Count} training data items for dataset {DatasetId}", trainingDataList.Count, datasetId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error importing training data for dataset {DatasetId}", datasetId);
                return false;
            }
        }

        public async Task<bool> TrainModelAsync(int datasetId, string modelName)
        {
            try
            {
                return await _sentimentAnalysisService.TrainModelAsync(datasetId, modelName);
            }
            catch (InvalidOperationException ex)
            {
                // 直接向上传递异常消息
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error training model for dataset {datasetId}");
                return false;
            }
        }

        public async Task<double> EvaluateModelAsync(int modelId)
        {
            try
            {
                var model = await _unitOfWork.MLModels.GetByIdAsync(modelId);
                if (model == null)
                {
                    _logger.LogWarning("Model {ModelId} not found", modelId);
                    return 0.0;
                }

                return await _sentimentAnalysisService.EvaluateModelAsync(modelId, model.DatasetId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error evaluating model {ModelId}", modelId);
                return 0.0;
            }
        }

        public async Task<IEnumerable<Dataset>> GetUserDatasetsAsync(int userId)
        {
            try
            {
                return await _unitOfWork.Datasets.FindAsync(d => d.UserId == userId && d.IsActive);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting datasets for user {UserId}", userId);
                return Enumerable.Empty<Dataset>();
            }
        }

        public async Task<IEnumerable<MLModel>> GetUserModelsAsync(int userId)
        {
            try
            {
                var userDatasets = await _unitOfWork.Datasets.FindAsync(d => d.UserId == userId && d.IsActive);
                var datasetIds = userDatasets.Select(d => d.Id).ToList();

                var models = await _unitOfWork.MLModels.FindAsync(m => datasetIds.Contains(m.DatasetId) && m.IsActive);
                return models;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting models for user {UserId}", userId);
                return Enumerable.Empty<MLModel>();
            }
        }

        public async Task<IEnumerable<TrainingData>> GetDatasetTrainingDataAsync(int datasetId)
        {
            try
            {
                return await _unitOfWork.TrainingData.FindAsync(td => td.DatasetId == datasetId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting training data for dataset {DatasetId}", datasetId);
                return Enumerable.Empty<TrainingData>();
            }
        }

        public async Task<bool> UpdateTrainingDataAsync(TrainingData data)
        {
            try
            {
                // 先查找数据库中的实体，避免EF Core多实例跟踪冲突
                var entity = await _unitOfWork.TrainingData.GetByIdAsync(data.Id);
                if (entity == null) return false;

                // 更新属性
                entity.Text = data.Text;
                entity.ActualSentiment = data.ActualSentiment;
                entity.CreatedAt = data.CreatedAt;

                _unitOfWork.TrainingData.Update(entity);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating training data {TrainingDataId}", data.Id);
                return false;
            }
        }

        public async Task<bool> DeleteTrainingDataAsync(int trainingDataId)
        {
            try
            {
                var data = await _unitOfWork.TrainingData.GetByIdAsync(trainingDataId);
                if (data == null) return false;
                _unitOfWork.TrainingData.Remove(data);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting training data {TrainingDataId}", trainingDataId);
                return false;
            }
        }

        public async Task<bool> AddTrainingDataAsync(int datasetId, string text, SentimentType sentiment)
        {
            try
            {
                var data = new TrainingData
                {
                    DatasetId = datasetId,
                    Text = text,
                    ActualSentiment = sentiment,
                    CreatedAt = DateTime.Now
                };
                await _unitOfWork.TrainingData.AddAsync(data);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding training data to dataset {DatasetId}", datasetId);
                return false;
            }
        }

        public async Task<bool> DeleteDatasetAsync(int datasetId, int userId)
        {
            try
            {
                var dataset = await _unitOfWork.Datasets.FirstOrDefaultAsync(d => d.Id == datasetId && d.UserId == userId);
                if (dataset == null)
                {
                    _logger.LogWarning("Dataset {DatasetId} not found for user {UserId}", datasetId, userId);
                    return false;
                }

                dataset.IsActive = false;
                _unitOfWork.Datasets.Update(dataset);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Dataset {DatasetId} deleted for user {UserId}", datasetId, userId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting dataset {DatasetId} for user {UserId}", datasetId, userId);
                return false;
            }
        }

        public async Task<bool> DeleteModelAsync(int modelId, int userId)
        {
            try
            {
                var model = await _unitOfWork.MLModels.GetByIdAsync(modelId);
                if (model == null)
                {
                    _logger.LogWarning("Model {ModelId} not found", modelId);
                    return false;
                }

                var dataset = await _unitOfWork.Datasets.GetByIdAsync(model.DatasetId);
                if (dataset == null || dataset.UserId != userId)
                {
                    _logger.LogWarning("Model {ModelId} does not belong to user {UserId}", modelId, userId);
                    return false;
                }

                // 删除模型文件
                if (!string.IsNullOrWhiteSpace(model.ModelPath) && File.Exists(model.ModelPath))
                {
                    try { File.Delete(model.ModelPath); }
                    catch (Exception ex) { _logger.LogWarning(ex, "Failed to delete model file: {Path}", model.ModelPath); }
                }

                // 物理删除数据库记录（级联删除分析历史）
                _unitOfWork.MLModels.Remove(model);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Model {ModelId} and related analysis deleted for user {UserId}", modelId, userId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting model {ModelId} for user {UserId}", modelId, userId);
                return false;
            }
        }

        public async Task<bool> LoadModelAsync(int modelId)
        {
            try
            {
                return await _sentimentAnalysisService.LoadModelAsync(modelId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading model {ModelId}", modelId);
                return false;
            }
        }

        public async Task<bool> UpdateDatasetAsync(int datasetId, string newName, string? newDescription)
        {
            try
            {
                var dataset = await _unitOfWork.Datasets.GetByIdAsync(datasetId);
                if (dataset == null)
                {
                    _logger.LogWarning("Dataset {DatasetId} not found", datasetId);
                    return false;
                }
                dataset.Name = newName;
                dataset.Description = newDescription;
                _unitOfWork.Datasets.Update(dataset);
                await _unitOfWork.SaveChangesAsync();
                _logger.LogInformation("Dataset {DatasetId} updated", datasetId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating dataset {DatasetId}", datasetId);
                return false;
            }
        }

        private async Task<List<(string text, SentimentType sentiment)>> ReadCsvFileAsync(string filePath)
        {
            var data = new List<(string text, SentimentType sentiment)>();
            var lines = await File.ReadAllLinesAsync(filePath);
            
            foreach (var line in lines.Skip(1)) // Skip header
            {
                var parts = line.Split(',');
                if (parts.Length >= 2)
                {
                    var text = parts[0].Trim('"');
                    var sentimentStr = parts[1].Trim('"');
                    
                    if (Enum.TryParse<SentimentType>(sentimentStr, out var sentiment))
                    {
                        data.Add((text, sentiment));
                    }
                }
            }

            return data;
        }

        private async Task<List<(string text, SentimentType sentiment)>> ReadJsonFileAsync(string filePath)
        {
            var data = new List<(string text, SentimentType sentiment)>();
            var json = await File.ReadAllTextAsync(filePath);
            
            try
            {
                var items = JsonSerializer.Deserialize<JsonElement[]>(json);
                foreach (var item in items)
                {
                    if (item.TryGetProperty("text", out var textElement) && 
                        item.TryGetProperty("sentiment", out var sentimentElement))
                    {
                        var text = textElement.GetString();
                        var sentimentStr = sentimentElement.GetString();
                        
                        if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(sentimentStr) &&
                            Enum.TryParse<SentimentType>(sentimentStr, out var sentiment))
                        {
                            data.Add((text, sentiment));
                        }
                    }
                }
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Error parsing JSON file {FilePath}", filePath);
            }

            return data;
        }

        private async Task<List<(string text, SentimentType sentiment)>> ReadTextFileAsync(string filePath)
        {
            var data = new List<(string text, SentimentType sentiment)>();
            var lines = await File.ReadAllLinesAsync(filePath);
            
            foreach (var line in lines)
            {
                var parts = line.Split('\t');
                if (parts.Length >= 2)
                {
                    var text = parts[0].Trim();
                    var sentimentStr = parts[1].Trim();
                    
                    if (Enum.TryParse<SentimentType>(sentimentStr, out var sentiment))
                    {
                        data.Add((text, sentiment));
                    }
                }
            }

            return data;
        }
    }
} 