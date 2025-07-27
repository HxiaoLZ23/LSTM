using Microsoft.ML;
using Microsoft.ML.Data;
using LSTM.ML.Interfaces;
using LSTM.ML.Models;
using LSTM.Models;
using LSTM.Models.DTOs;
using LSTM.Data.Interfaces;
using Microsoft.Extensions.Logging;

namespace LSTM.ML.Services
{
    public class SentimentAnalysisService : ISentimentAnalysisService
    {
        private readonly MLContext _mlContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SentimentAnalysisService> _logger;
        private ITransformer? _model;
        private PredictionEngine<SentimentDataMultiClass, SentimentPredictionMultiClass>? _predictionEngine;

        public SentimentAnalysisService(IUnitOfWork unitOfWork, ILogger<SentimentAnalysisService> logger)
        {
            _mlContext = new MLContext(seed: 0);
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<SentimentAnalysisResultDto> AnalyzeSentimentAsync(string text)
        {
            try
            {
                if (_model == null)
                {
                    // 使用默认模型或最新训练的模型
                    var latestModel = await _unitOfWork.MLModels.FirstOrDefaultAsync(m => m.IsActive && m.Status == ModelStatus.Trained);
                    if (latestModel != null)
                    {
                        await LoadModelAsync(latestModel.Id);
                    }
                    else
                    {
                        // 如果没有训练好的模型，使用简单的规则分析
                        return await AnalyzeWithRulesAsync(text);
                    }
                }

                if (_model == null)
                {
                    return await AnalyzeWithRulesAsync(text);
                }

                var input = new SentimentDataMultiClass { Text = text };
                // 每次推理都新建PredictionEngine，避免缓存导致结果不变
                var predictionEngine = _mlContext.Model.CreatePredictionEngine<SentimentDataMultiClass, SentimentPredictionMultiClass>(_model);
                var prediction = predictionEngine.Predict(input);

                var sentiment = prediction.PredictedLabel switch
                {
                    // 基础情感
                    "Positive" => SentimentType.Positive,
                    "Negative" => SentimentType.Negative,
                    "Neutral" => SentimentType.Neutral,
                    
                    // 扩展情感类别
                    "Joy" => SentimentType.Joy,
                    "Anger" => SentimentType.Anger,
                    "Sadness" => SentimentType.Sadness,
                    "Fear" => SentimentType.Fear,
                    "Surprise" => SentimentType.Surprise,
                    "Disgust" => SentimentType.Disgust,
                    "Love" => SentimentType.Love,
                    "Hate" => SentimentType.Hate,
                    "Anxiety" => SentimentType.Anxiety,
                    "Excitement" => SentimentType.Excitement,
                    "Disappointment" => SentimentType.Disappointment,
                    "Satisfaction" => SentimentType.Satisfaction,
                    "Confusion" => SentimentType.Confusion,
                    "Gratitude" => SentimentType.Gratitude,
                    "Enthusiasm" => SentimentType.Enthusiasm,
                    "Frustration" => SentimentType.Frustration,
                    "Relief" => SentimentType.Relief,
                    "Curiosity" => SentimentType.Curiosity,
                    "Pride" => SentimentType.Pride,
                    "Shame" => SentimentType.Shame,
                    "Hope" => SentimentType.Hope,
                    "Despair" => SentimentType.Despair,
                    "Admiration" => SentimentType.Admiration,
                    "Contempt" => SentimentType.Contempt,
                    "Amusement" => SentimentType.Amusement,
                    "Boredom" => SentimentType.Boredom,
                    "Nostalgia" => SentimentType.Nostalgia,
                    "Optimism" => SentimentType.Optimism,
                    "Pessimism" => SentimentType.Pessimism,
                    _ => SentimentType.Neutral
                };

                var confidence = prediction.Score.Max();

                return new SentimentAnalysisResultDto
                {
                    Text = text,
                    PredictedSentiment = sentiment,
                    Confidence = confidence,
                    AnalyzedAt = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error analyzing sentiment for text: {Text}", text);
                return await AnalyzeWithRulesAsync(text);
            }
        }

        public async Task<bool> TrainModelAsync(int datasetId, string modelName)
        {
            try
            {
                _logger.LogInformation("Starting model training for dataset {DatasetId}", datasetId);

                // 获取训练数据
                var trainingData = await _unitOfWork.TrainingData.FindAsync(td => td.DatasetId == datasetId);
                if (!trainingData.Any())
                {
                    _logger.LogWarning("No training data found for dataset {DatasetId}", datasetId);
                    throw new InvalidOperationException($"训练数据为空，无法训练模型！");
                }

                // 统计类别分布
                var labelGroups = trainingData.GroupBy(td => td.ActualSentiment.ToString())
                    .Select(g => new { Label = g.Key, Count = g.Count() })
                    .ToList();
                int total = trainingData.Count();
                int minCount = labelGroups.Min(g => g.Count);
                int maxCount = labelGroups.Max(g => g.Count);
                var zeroLabels = System.Enum.GetNames(typeof(LSTM.Models.SentimentType))
                    .Except(labelGroups.Select(g => g.Label)).ToList();

                // 样本过少或有类别为0，阻止训练
                if (total < 10 || zeroLabels.Count > 0)
                {
                    string msg = $"当前数据集总样本数：{total}\n" +
                        string.Join("\n", labelGroups.Select(g => $"类别 {g.Label}：{g.Count} 条")) +
                        (zeroLabels.Count > 0 ? "\n\n以下类别无样本：" + string.Join(", ", zeroLabels) : "") +
                        "\n\n请补充数据后再训练。";
                    throw new InvalidOperationException(msg);
                }

                // 极度不均衡，警告但允许继续
                if (maxCount > minCount * 10)
                {
                    string msg = $"当前数据集类别分布极度不均衡：\n" +
                        string.Join("\n", labelGroups.Select(g => $"类别 {g.Label}：{g.Count} 条")) +
                        "\n\n建议补充样本使各类别均衡，否则模型效果可能极差。";
                    throw new InvalidOperationException(msg);
                }

                // 转换数据格式
                var mlData = trainingData.Select(td => new SentimentDataMultiClass
                {
                    Text = td.Text,
                    Label = td.ActualSentiment.ToString()
                }).ToArray();

                var dataView = _mlContext.Data.LoadFromEnumerable(mlData);

                // 创建训练管道
                var pipeline = _mlContext.Transforms.Conversion.MapValueToKey("Label", "Label")
                    .Append(_mlContext.Transforms.Text.FeaturizeText("Features", "Text"))
                    .Append(_mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy("Label", "Features"))
                    .Append(_mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

                // 训练模型
                _model = pipeline.Fit(dataView);

                // 保存模型
                var modelPath = Path.Combine("Models", $"{modelName}_{DateTime.Now:yyyyMMdd_HHmmss}.zip");
                Directory.CreateDirectory("Models");
                
                _mlContext.Model.Save(_model, dataView.Schema, modelPath);

                // 评估模型
                var testData = _mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
                var predictions = _model.Transform(testData.TestSet);
                var metrics = _mlContext.MulticlassClassification.Evaluate(predictions);

                // 创建模型记录
                var dataset = await _unitOfWork.Datasets.GetByIdAsync(datasetId);
                var mlModel = new MLModel
                {
                    Name = modelName,
                    Description = $"使用数据集 {dataset?.Name} 训练的情感分析模型",
                    ModelPath = modelPath,
                    Status = ModelStatus.Trained,
                    Accuracy = metrics.MacroAccuracy,
                    TrainedAt = DateTime.Now,
                    DatasetId = datasetId,
                    IsActive = true
                };

                await _unitOfWork.MLModels.AddAsync(mlModel);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Model training completed. Accuracy: {Accuracy:P2}", metrics.MacroAccuracy);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error training model for dataset {DatasetId}", datasetId);
                return false;
            }
        }

        public async Task<bool> LoadModelAsync(int modelId)
        {
            try
            {
                var model = await _unitOfWork.MLModels.GetByIdAsync(modelId);
                if (model == null || !File.Exists(model.ModelPath))
                {
                    _logger.LogWarning("Model {ModelId} not found or file doesn't exist", modelId);
                    return false;
                }

                _model = _mlContext.Model.Load(model.ModelPath, out var schema);
                _predictionEngine = _mlContext.Model.CreatePredictionEngine<SentimentDataMultiClass, SentimentPredictionMultiClass>(_model);

                _logger.LogInformation("Model {ModelId} loaded successfully", modelId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading model {ModelId}", modelId);
                return false;
            }
        }

        public async Task<double> EvaluateModelAsync(int modelId, int datasetId)
        {
            try
            {
                if (!await LoadModelAsync(modelId))
                {
                    return 0.0;
                }

                var testData = await _unitOfWork.TrainingData.FindAsync(td => td.DatasetId == datasetId);
                if (!testData.Any())
                {
                    return 0.0;
                }

                var mlData = testData.Select(td => new SentimentDataMultiClass
                {
                    Text = td.Text,
                    Label = td.ActualSentiment.ToString()
                }).ToArray();

                var dataView = _mlContext.Data.LoadFromEnumerable(mlData);
                var predictions = _model!.Transform(dataView);
                var metrics = _mlContext.MulticlassClassification.Evaluate(predictions);

                return metrics.MacroAccuracy;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error evaluating model {ModelId}", modelId);
                return 0.0;
            }
        }

        public async Task<IEnumerable<SentimentAnalysisResultDto>> BatchAnalyzeAsync(IEnumerable<string> texts)
        {
            var results = new List<SentimentAnalysisResultDto>();
            
            foreach (var text in texts)
            {
                var result = await AnalyzeSentimentAsync(text);
                results.Add(result);
            }

            return results;
        }

        public async Task<bool> IsModelLoadedAsync()
        {
            return await Task.FromResult(_predictionEngine != null);
        }

        private async Task<SentimentAnalysisResultDto> AnalyzeWithRulesAsync(string text)
        {
            await Task.Delay(0); // 保持异步接口一致性

            // 扩展的规则分析
            var emotionKeywords = new Dictionary<SentimentType, string[]>
            {
                { SentimentType.Positive, new[] { "好", "棒", "喜欢", "满意", "优秀", "赞", "很好", "非常好", "完美", "推荐" } },
                { SentimentType.Negative, new[] { "差", "坏", "不好", "失望", "糟糕", "讨厌", "不满", "垃圾", "不推荐", "问题" } },
                { SentimentType.Joy, new[] { "开心", "快乐", "高兴", "兴奋", "喜悦", "欢乐", "愉快", "爽", "爽快", "痛快" } },
                { SentimentType.Anger, new[] { "愤怒", "生气", "恼火", "气愤", "暴怒", "发火", "火大", "气死", "气炸", "愤怒" } },
                { SentimentType.Sadness, new[] { "悲伤", "难过", "伤心", "痛苦", "悲哀", "沮丧", "失落", "心碎", "泪", "哭" } },
                { SentimentType.Fear, new[] { "害怕", "恐惧", "担心", "担心", "恐慌", "惊慌", "害怕", "恐怖", "吓人", "可怕" } },
                { SentimentType.Surprise, new[] { "惊讶", "震惊", "意外", "没想到", "居然", "竟然", "吃惊", "惊奇", "意外", "突然" } },
                { SentimentType.Disgust, new[] { "恶心", "厌恶", "讨厌", "反感", "嫌弃", "恶心", "想吐", "反胃", "恶心", "厌恶" } },
                { SentimentType.Love, new[] { "爱", "喜欢", "爱慕", "爱恋", "爱情", "爱心", "爱意", "爱戴", "爱惜", "爱护" } },
                { SentimentType.Hate, new[] { "恨", "讨厌", "憎恨", "厌恶", "仇恨", "痛恨", "憎恶", "恨死", "恨透", "恨意" } },
                { SentimentType.Anxiety, new[] { "焦虑", "担心", "忧虑", "不安", "紧张", "焦虑", "担心", "忧虑", "不安", "紧张" } },
                { SentimentType.Excitement, new[] { "兴奋", "激动", "兴奋", "激动", "兴奋", "激动", "兴奋", "激动", "兴奋", "激动" } },
                { SentimentType.Disappointment, new[] { "失望", "沮丧", "失落", "失望", "沮丧", "失落", "失望", "沮丧", "失落", "失望" } },
                { SentimentType.Satisfaction, new[] { "满意", "满足", "满意", "满足", "满意", "满足", "满意", "满足", "满意", "满足" } },
                { SentimentType.Confusion, new[] { "困惑", "迷茫", "疑惑", "困惑", "迷茫", "疑惑", "困惑", "迷茫", "疑惑", "困惑" } },
                { SentimentType.Gratitude, new[] { "感谢", "感激", "谢谢", "感谢", "感激", "谢谢", "感谢", "感激", "谢谢", "感谢" } },
                { SentimentType.Enthusiasm, new[] { "热情", "积极", "热情", "积极", "热情", "积极", "热情", "积极", "热情", "积极" } },
                { SentimentType.Frustration, new[] { "沮丧", "挫败", "沮丧", "挫败", "沮丧", "挫败", "沮丧", "挫败", "沮丧", "挫败" } },
                { SentimentType.Relief, new[] { "宽慰", "放心", "宽慰", "放心", "宽慰", "放心", "宽慰", "放心", "宽慰", "放心" } },
                { SentimentType.Curiosity, new[] { "好奇", "好奇", "好奇", "好奇", "好奇", "好奇", "好奇", "好奇", "好奇", "好奇" } },
                { SentimentType.Pride, new[] { "自豪", "骄傲", "自豪", "骄傲", "自豪", "骄傲", "自豪", "骄傲", "自豪", "骄傲" } },
                { SentimentType.Shame, new[] { "羞愧", "羞耻", "羞愧", "羞耻", "羞愧", "羞耻", "羞愧", "羞耻", "羞愧", "羞耻" } },
                { SentimentType.Hope, new[] { "希望", "期待", "希望", "期待", "希望", "期待", "希望", "期待", "希望", "期待" } },
                { SentimentType.Despair, new[] { "绝望", "绝望", "绝望", "绝望", "绝望", "绝望", "绝望", "绝望", "绝望", "绝望" } },
                { SentimentType.Admiration, new[] { "钦佩", "敬佩", "钦佩", "敬佩", "钦佩", "敬佩", "钦佩", "敬佩", "钦佩", "敬佩" } },
                { SentimentType.Contempt, new[] { "轻蔑", "鄙视", "轻蔑", "鄙视", "轻蔑", "鄙视", "轻蔑", "鄙视", "轻蔑", "鄙视" } },
                { SentimentType.Amusement, new[] { "娱乐", "有趣", "娱乐", "有趣", "娱乐", "有趣", "娱乐", "有趣", "娱乐", "有趣" } },
                { SentimentType.Boredom, new[] { "无聊", "无聊", "无聊", "无聊", "无聊", "无聊", "无聊", "无聊", "无聊", "无聊" } },
                { SentimentType.Nostalgia, new[] { "怀旧", "回忆", "怀旧", "回忆", "怀旧", "回忆", "怀旧", "回忆", "怀旧", "回忆" } },
                { SentimentType.Optimism, new[] { "乐观", "乐观", "乐观", "乐观", "乐观", "乐观", "乐观", "乐观", "乐观", "乐观" } },
                { SentimentType.Pessimism, new[] { "悲观", "悲观", "悲观", "悲观", "悲观", "悲观", "悲观", "悲观", "悲观", "悲观" } }
            };

            var textLower = text.ToLower();
            var emotionScores = new Dictionary<SentimentType, int>();

            // 计算每种情感的得分
            foreach (var emotion in emotionKeywords)
            {
                var score = emotion.Value.Count(word => textLower.Contains(word));
                emotionScores[emotion.Key] = score;
            }

            // 找到得分最高的情感
            var maxScore = emotionScores.Values.Max();
            var predictedEmotion = emotionScores.FirstOrDefault(x => x.Value == maxScore).Key;

            SentimentType sentiment;
            double confidence;

            if (maxScore > 0)
            {
                sentiment = predictedEmotion;
                confidence = 0.5 + (maxScore * 0.1);
            }
            else
            {
                sentiment = SentimentType.Neutral;
                confidence = 0.5;
            }

            confidence = Math.Min(confidence, 1.0);

            return new SentimentAnalysisResultDto
            {
                Text = text,
                PredictedSentiment = sentiment,
                Confidence = confidence,
                AnalyzedAt = DateTime.Now
            };
        }
    }
} 