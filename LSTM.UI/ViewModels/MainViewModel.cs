using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using LSTM.Business.Interfaces;
using LSTM.Models;
using LSTM.Models.DTOs;
using LSTM.UI.Commands;
using Microsoft.Extensions.Logging;
using LSTM.UI.Views;
using System.Linq;
using System.Collections.Generic;

namespace LSTM.UI.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ISentimentAnalysisBusinessService _sentimentAnalysisService;
        private readonly IModelTrainingService _modelTrainingService;
        private readonly ILogger<MainViewModel> _logger;
        
        private User? _currentUser;
        private string _inputText = string.Empty;
        private SentimentAnalysisResultDto? _currentResult;
        private bool _isAnalyzing = false;
        private int _selectedTabIndex = 0;
        
        // 训练相关
        private string _newDatasetName = string.Empty;
        private string _newDatasetDescription = string.Empty;
        private string _newModelName = string.Empty;
        private Dataset? _selectedDataset;
        private MLModel? _selectedModel;
        private bool _isTraining = false;

        private ObservableCollection<TrainingData> _trainingDataList = new();
        public ObservableCollection<TrainingData> TrainingDataList
        {
            get => _trainingDataList;
            set => SetProperty(ref _trainingDataList, value);
        }
        private TrainingData? _selectedTrainingData;
        public TrainingData? SelectedTrainingData
        {
            get => _selectedTrainingData;
            set => SetProperty(ref _selectedTrainingData, value);
        }
        public ICommand LoadTrainingDataCommand { get; private set; } = null!;
        public ICommand SaveTrainingDataCommand { get; private set; } = null!;
        public ICommand DeleteTrainingDataCommand { get; private set; } = null!;
        public ICommand AddTrainingDataCommand { get; private set; } = null!;
        public ICommand ImportTrainingDataCommand { get; private set; } = null!;
        public ICommand DeleteDatasetCommand { get; private set; } = null!;

        public MainViewModel(
            ISentimentAnalysisBusinessService sentimentAnalysisService,
            IModelTrainingService modelTrainingService,
            ILogger<MainViewModel> logger)
        {
            _sentimentAnalysisService = sentimentAnalysisService;
            _modelTrainingService = modelTrainingService;
            _logger = logger;
            
            AnalysisHistory = new ObservableCollection<SentimentAnalysisResultDto>();
            Datasets = new ObservableCollection<Dataset>();
            Models = new ObservableCollection<MLModel>();
            
            InitializeCommands();
        }

        public User? CurrentUser
        {
            get => _currentUser;
            set => SetProperty(ref _currentUser, value);
        }

        public string InputText
        {
            get => _inputText;
            set => SetProperty(ref _inputText, value);
        }

        public SentimentAnalysisResultDto? CurrentResult
        {
            get => _currentResult;
            set => SetProperty(ref _currentResult, value);
        }

        public bool IsAnalyzing
        {
            get => _isAnalyzing;
            set => SetProperty(ref _isAnalyzing, value);
        }

        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set => SetProperty(ref _selectedTabIndex, value);
        }

        public ObservableCollection<SentimentAnalysisResultDto> AnalysisHistory { get; }
        public ObservableCollection<Dataset> Datasets { get; }
        public ObservableCollection<MLModel> Models { get; }

        // 训练相关属性
        public string NewDatasetName
        {
            get => _newDatasetName;
            set => SetProperty(ref _newDatasetName, value);
        }

        public string NewDatasetDescription
        {
            get => _newDatasetDescription;
            set => SetProperty(ref _newDatasetDescription, value);
        }

        public string NewModelName
        {
            get => _newModelName;
            set => SetProperty(ref _newModelName, value);
        }

        public Dataset? SelectedDataset
        {
            get => _selectedDataset;
            set => SetProperty(ref _selectedDataset, value);
        }

        public MLModel? SelectedModel
        {
            get => _selectedModel;
            set => SetProperty(ref _selectedModel, value);
        }

        public bool IsTraining
        {
            get => _isTraining;
            set => SetProperty(ref _isTraining, value);
        }

        // 命令
        public ICommand AnalyzeCommand { get; private set; } = null!;
        public ICommand ClearCommand { get; private set; } = null!;
        public ICommand LoadHistoryCommand { get; private set; } = null!;
        public ICommand CreateDatasetCommand { get; private set; } = null!;
        public ICommand TrainModelCommand { get; private set; } = null!;
        public ICommand LoadModelCommand { get; private set; } = null!;
        public ICommand RefreshDatasetsCommand { get; private set; } = null!;
        public ICommand RefreshModelsCommand { get; private set; } = null!;
        public ICommand DeleteModelCommand { get; private set; } = null!;
        public ICommand LogoutCommand { get; private set; } = null!;
        public ICommand EditDatasetCommand { get; private set; } = null!;
        public ICommand EditDatasetContentCommand { get; private set; } = null!;
        private string _editDatasetName = string.Empty;
        private string? _editDatasetDescription;
        private Dataset? _editingDataset;
        public string EditDatasetName
        {
            get => _editDatasetName;
            set => SetProperty(ref _editDatasetName, value);
        }
        public string? EditDatasetDescription
        {
            get => _editDatasetDescription;
            set => SetProperty(ref _editDatasetDescription, value);
        }
        public Dataset? EditingDataset
        {
            get => _editingDataset;
            set => SetProperty(ref _editingDataset, value);
        }

        private void InitializeCommands()
        {
            AnalyzeCommand = new RelayCommand(async () => await AnalyzeTextAsync(), 
                () => !IsAnalyzing && !string.IsNullOrWhiteSpace(InputText));
            
            ClearCommand = new RelayCommand(ClearText);
            
            LoadHistoryCommand = new RelayCommand(async () => await LoadAnalysisHistoryAsync());
            
            CreateDatasetCommand = new RelayCommand(async () => await CreateDatasetAsync(),
                () => !string.IsNullOrWhiteSpace(NewDatasetName));
            
            TrainModelCommand = new RelayCommand(async () => await TrainModelAsync(),
                () => !IsTraining && SelectedDataset != null && !string.IsNullOrWhiteSpace(NewModelName));
            
            LoadModelCommand = new RelayCommand(async () => await LoadModelAsync(),
                () => SelectedModel != null);
            
            RefreshDatasetsCommand = new RelayCommand(async () => await RefreshDatasetsAsync());
            
            RefreshModelsCommand = new RelayCommand(async () => await RefreshModelsAsync());
            
            DeleteModelCommand = new RelayCommand(async () => await DeleteModelAsync(),
                () => SelectedModel != null);
            
            LogoutCommand = new RelayCommand(Logout);
            EditDatasetCommand = new RelayCommand<Dataset>(OnEditDataset);
            EditDatasetContentCommand = new RelayCommand<Dataset>(OnEditDatasetContent);
            LoadTrainingDataCommand = new RelayCommand(async () => await LoadTrainingDataAsync(), () => SelectedDataset != null);
            SaveTrainingDataCommand = new RelayCommand(async () => await SaveTrainingDataAsync(), () => SelectedDataset != null);
            DeleteTrainingDataCommand = new RelayCommand(async () => await DeleteTrainingDataAsync(), () => SelectedTrainingData != null);
            AddTrainingDataCommand = new RelayCommand(async () => await AddTrainingDataAsync(), () => SelectedDataset != null);
            ImportTrainingDataCommand = new RelayCommand<Dataset>(async d => await OnImportTrainingData(d));
            DeleteDatasetCommand = new RelayCommand<Dataset>(async d => await DeleteDatasetAsync(d), d => d != null);
        }
        private void OnEditDataset(Dataset? dataset)
        {
            if (dataset == null) return;
            if (Application.Current.Windows.OfType<MainWindow>().FirstOrDefault() is MainWindow mainWindow)
            {
                mainWindow.ShowEditDatasetDialog(dataset);
            }
        }

        private async Task AnalyzeTextAsync()
        {
            if (CurrentUser == null) return;

            try
            {
                IsAnalyzing = true;
                
                var result = await _sentimentAnalysisService.AnalyzeTextAsync(InputText, CurrentUser.Id);
                CurrentResult = result;
                
                // 添加到历史记录
                AnalysisHistory.Insert(0, result);
                
                _logger.LogInformation("Text analysis completed for user {UserId}", CurrentUser.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error analyzing text");
                MessageBox.Show("分析时发生错误，请稍后重试", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsAnalyzing = false;
            }
        }

        private void ClearText()
        {
            InputText = string.Empty;
            CurrentResult = null;
        }

        private async Task LoadAnalysisHistoryAsync()
        {
            if (CurrentUser == null) return;

            try
            {
                var history = await _sentimentAnalysisService.GetUserAnalysisHistoryAsync(CurrentUser.Id);
                AnalysisHistory.Clear();
                
                foreach (var item in history)
                {
                    AnalysisHistory.Add(item);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading analysis history");
                MessageBox.Show("加载历史记录时发生错误", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task CreateDatasetAsync()
        {
            if (CurrentUser == null) return;

            try
            {
                var success = await _modelTrainingService.CreateDatasetAsync(
                    NewDatasetName, 
                    NewDatasetDescription, 
                    CurrentUser.Id);
                
                if (success)
                {
                    NewDatasetName = string.Empty;
                    NewDatasetDescription = string.Empty;
                    await RefreshDatasetsAsync();
                    MessageBox.Show("数据集创建成功！", "成功", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("数据集创建失败", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating dataset");
                MessageBox.Show("创建数据集时发生错误", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task TrainModelAsync()
        {
            if (SelectedDataset == null) return;

            try
            {
                IsTraining = true;
                var success = await _modelTrainingService.TrainModelAsync(SelectedDataset.Id, NewModelName);
                if (success)
                {
                    NewModelName = string.Empty;
                    await RefreshModelsAsync();
                    MessageBox.Show("模型训练成功！", "成功", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("模型训练失败", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "训练中止", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error training model");
                MessageBox.Show("训练模型时发生错误", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsTraining = false;
            }
        }

        private async Task LoadModelAsync()
        {
            if (SelectedModel == null) return;

            try
            {
                var success = await _modelTrainingService.LoadModelAsync(SelectedModel.Id);
                
                if (success)
                {
                    MessageBox.Show("模型加载成功！", "成功", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("模型加载失败", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading model");
                MessageBox.Show("加载模型时发生错误", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task RefreshDatasetsAsync()
        {
            if (CurrentUser == null) return;

            try
            {
                var datasets = await _modelTrainingService.GetUserDatasetsAsync(CurrentUser.Id);
                Datasets.Clear();
                
                foreach (var dataset in datasets)
                {
                    Datasets.Add(dataset);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error refreshing datasets");
            }
        }

        private async Task RefreshModelsAsync()
        {
            if (CurrentUser == null) return;

            try
            {
                var models = await _modelTrainingService.GetUserModelsAsync(CurrentUser.Id);
                Models.Clear();
                
                foreach (var model in models)
                {
                    Models.Add(model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error refreshing models");
            }
        }

        private async Task DeleteModelAsync()
        {
            if (SelectedModel == null) return;

            var result = MessageBox.Show(
                $"确定要删除模型 '{SelectedModel.Name}' 吗？\n\n此操作将：\n• 永久删除模型文件\n• 删除所有相关的分析历史记录\n• 无法撤销",
                "确认删除模型",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes) return;

            try
            {
                var success = await _modelTrainingService.DeleteModelAsync(SelectedModel.Id, CurrentUser!.Id);
                
                if (success)
                {
                    var deletedModel = SelectedModel;
                    Models.Remove(deletedModel);
                    SelectedModel = null;
                    MessageBox.Show("模型删除成功！", "成功", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("模型删除失败", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting model");
                MessageBox.Show("删除模型时发生错误", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task EditDatasetAsync()
        {
            if (EditingDataset == null) return;
            var success = await _modelTrainingService.UpdateDatasetAsync(EditingDataset.Id, EditDatasetName, EditDatasetDescription);
            if (success)
            {
                EditingDataset.Name = EditDatasetName;
                EditingDataset.Description = EditDatasetDescription;
                await RefreshDatasetsAsync();
                MessageBox.Show("数据集修改成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("数据集修改失败！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Logout()
        {
            CurrentUser = null;
            Application.Current.MainWindow?.Close();
            
            // 重新显示登录窗口
            var loginWindow = App.GetService<Views.LoginWindow>();
            loginWindow.Show();
        }

        public async Task InitializeAsync()
        {
            await LoadAnalysisHistoryAsync();
            await RefreshDatasetsAsync();
            await RefreshModelsAsync();
        }

        private async Task LoadTrainingDataAsync()
        {
            TrainingDataList.Clear();
            if (SelectedDataset == null) return;
            var data = await _modelTrainingService.GetDatasetTrainingDataAsync(SelectedDataset.Id);
            foreach (var item in data)
            {
                TrainingDataList.Add(item);
            }
        }
        private async Task SaveTrainingDataAsync()
        {
            bool allSuccess = true;
            foreach (var item in TrainingDataList)
            {
                var success = await _modelTrainingService.UpdateTrainingDataAsync(item);
                if (!success) allSuccess = false;
            }
            if (allSuccess)
                MessageBox.Show("保存成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show("部分数据保存失败！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private async Task DeleteTrainingDataAsync()
        {
            if (SelectedTrainingData == null) return;
            var result = MessageBox.Show("确定要删除该条训练数据吗？", "确认", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;
            var success = await _modelTrainingService.DeleteTrainingDataAsync(SelectedTrainingData.Id);
            if (success)
            {
                TrainingDataList.Remove(SelectedTrainingData);
                MessageBox.Show("删除成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("删除失败！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async Task AddTrainingDataAsync()
        {
            if (SelectedDataset == null) return;
            var newData = new TrainingData
            {
                DatasetId = SelectedDataset.Id,
                Text = "",
                ActualSentiment = SentimentType.Neutral,
                CreatedAt = DateTime.Now
            };
            var success = await _modelTrainingService.AddTrainingDataAsync(newData.DatasetId, newData.Text, newData.ActualSentiment);
            if (success)
            {
                await LoadTrainingDataAsync();
                MessageBox.Show("新增成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("新增失败！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnEditDatasetContent(Dataset? dataset)
        {
            if (dataset == null) return;
            var service = App.GetService<IModelTrainingService>();
            var win = new LSTM.UI.Views.EditDatasetContentWindow(service, dataset.Id)
            {
                Owner = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive)
            };
            win.ShowDialog();
        }

        private async Task OnImportTrainingData(Dataset? dataset)
        {
            if (dataset == null) return;
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Title = "导入训练数据",
                Filter = "CSV文件|*.csv|文本文件|*.txt|JSON文件|*.json|所有文件|*.*"
            };
            if (dialog.ShowDialog() == true)
            {
                var filePath = dialog.FileName;
                var service = App.GetService<IModelTrainingService>();
                var result = await service.ImportTrainingDataAsync(dataset.Id, filePath);
                if (result)
                {
                    MessageBox.Show("导入成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    // 可选：自动刷新数据集内容
                }
                else
                {
                    MessageBox.Show("导入失败，请检查文件格式！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async Task DeleteDatasetAsync(Dataset? dataset)
        {
            if (dataset == null || CurrentUser == null) return;
            var result = MessageBox.Show($"确定要删除数据集 '{dataset.Name}' 吗？\n\n此操作将删除该数据集及其所有训练数据，无法恢复。", "确认删除数据集", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result != MessageBoxResult.Yes) return;
            var success = await _modelTrainingService.DeleteDatasetAsync(dataset.Id, CurrentUser.Id);
            if (success)
            {
                Datasets.Remove(dataset);
                if (SelectedDataset == dataset) SelectedDataset = null;
                MessageBox.Show("数据集删除成功！", "成功", MessageBoxButton.OK, MessageBoxImage.Information);
                await RefreshDatasetsAsync();
            }
            else
            {
                MessageBox.Show("数据集删除失败！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // 自动补全缺失类别训练数据
        public async Task AutoCompleteMissingSentimentDataAsync()
        {
            if (SelectedDataset == null) return;
            // 统计当前数据集的类别分布
            var data = await _modelTrainingService.GetDatasetTrainingDataAsync(SelectedDataset.Id);
            var allLabels = System.Enum.GetValues(typeof(SentimentType)).Cast<SentimentType>().ToList();
            var labelGroups = data.GroupBy(td => td.ActualSentiment).ToDictionary(g => g.Key, g => g.Count());
            var missingLabels = allLabels.Where(l => !labelGroups.ContainsKey(l) || labelGroups[l] == 0).ToList();
            if (missingLabels.Count == 0)
            {
                MessageBox.Show("所有情感类别均有样本，无需补全。", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            // 生成每个缺失类别的样本
            var newData = new List<(string text, SentimentType sentiment)>();
            foreach (var label in missingLabels)
            {
                for (int i = 1; i <= 40; i++)
                {
                    string text = label switch
                    {
                        SentimentType.Fear => $"听到那个声音我感到害怕 ({i})",
                        SentimentType.Joy => $"我感到非常开心 ({i})",
                        SentimentType.Love => $"我很爱我的家人 ({i})",
                        SentimentType.Hate => $"我讨厌下雨天 ({i})",
                        SentimentType.Anger => $"他让我很生气 ({i})",
                        SentimentType.Sadness => $"我今天很难过 ({i})",
                        SentimentType.Surprise => $"这个消息让我很惊讶 ({i})",
                        SentimentType.Disgust => $"我对这个味道感到恶心 ({i})",
                        SentimentType.Anxiety => $"我最近总是很焦虑 ({i})",
                        SentimentType.Excitement => $"我对明天的旅行很兴奋 ({i})",
                        SentimentType.Disappointment => $"结果让我很失望 ({i})",
                        SentimentType.Satisfaction => $"我对服务很满意 ({i})",
                        SentimentType.Confusion => $"我有点困惑不解 ({i})",
                        SentimentType.Gratitude => $"谢谢你的帮助，我很感激 ({i})",
                        SentimentType.Enthusiasm => $"我对新项目充满热情 ({i})",
                        SentimentType.Frustration => $"事情进展不顺让我很沮丧 ({i})",
                        SentimentType.Relief => $"听到好消息我松了口气 ({i})",
                        SentimentType.Curiosity => $"我对这个问题很好奇 ({i})",
                        SentimentType.Pride => $"我为自己感到自豪 ({i})",
                        SentimentType.Shame => $"我为自己的错误感到羞愧 ({i})",
                        SentimentType.Hope => $"我对未来充满希望 ({i})",
                        SentimentType.Despair => $"我感到非常绝望 ({i})",
                        SentimentType.Admiration => $"我很钦佩她的勇气 ({i})",
                        SentimentType.Contempt => $"他的话让我感到轻蔑 ({i})",
                        SentimentType.Amusement => $"这个笑话很有趣 ({i})",
                        SentimentType.Boredom => $"我觉得很无聊 ({i})",
                        SentimentType.Nostalgia => $"看到老照片很怀旧 ({i})",
                        SentimentType.Optimism => $"我对结果很乐观 ({i})",
                        SentimentType.Pessimism => $"我对前景很悲观 ({i})",
                        SentimentType.Positive => $"今天心情很好 ({i})",
                        SentimentType.Negative => $"我不喜欢这种感觉 ({i})",
                        SentimentType.Neutral => $"天气一般般 ({i})",
                        _ => $"我感到{label} ({i})"
                    };
                    newData.Add((text, label));
                }
            }
            // 导入补全数据
            var result = await _modelTrainingService.ImportTrainingDataFromTextAsync(SelectedDataset.Id, newData);
            if (result)
            {
                MessageBox.Show($"已自动补全以下类别：{string.Join(", ", missingLabels)}，每类40条。请重新训练模型。", "补全成功", MessageBoxButton.OK, MessageBoxImage.Information);
                await LoadTrainingDataAsync();
            }
            else
            {
                MessageBox.Show("补全数据导入失败！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
} 