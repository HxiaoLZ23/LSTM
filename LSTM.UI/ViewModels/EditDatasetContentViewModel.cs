using System.Collections.ObjectModel;
using System.Windows.Input;
using LSTM.Models;
using LSTM.Business.Interfaces;
using LSTM.UI.Commands;
using System.Windows;
using System.Threading.Tasks;
using System.Linq;

namespace LSTM.UI.ViewModels
{
    public class TrainingDataEditItem : ViewModelBase
    {
        public int? Id { get; set; } // null表示新建
        private string _text = string.Empty;
        public string Text { get => _text; set => SetProperty(ref _text, value); }
        private SentimentType _sentiment = SentimentType.Neutral;
        public SentimentType Sentiment { get => _sentiment; set => SetProperty(ref _sentiment, value); }
        public bool IsDeleted { get; set; } = false;
    }

    public class EditDatasetContentViewModel : ViewModelBase
    {
        private readonly IModelTrainingService _service;
        private readonly int _datasetId;
        public ObservableCollection<TrainingDataEditItem> Items { get; } = new();
        public ICommand AddItemCommand { get; }
        public ICommand DeleteItemCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public EditDatasetContentViewModel(IModelTrainingService service, int datasetId)
        {
            _service = service;
            _datasetId = datasetId;
            AddItemCommand = new RelayCommand(AddItem);
            DeleteItemCommand = new RelayCommand<TrainingDataEditItem>(DeleteItem);
            SaveCommand = new RelayCommand(async () => await SaveAsync());
            CancelCommand = new RelayCommand<Window>(w => w?.Close());
        }
        public async Task LoadAsync()
        {
            Items.Clear();
            var data = await _service.GetDatasetTrainingDataAsync(_datasetId);
            foreach (var d in data)
            {
                Items.Add(new TrainingDataEditItem { Id = d.Id, Text = d.Text, Sentiment = d.ActualSentiment });
            }
        }
        private void AddItem()
        {
            Items.Add(new TrainingDataEditItem());
        }
        private void DeleteItem(TrainingDataEditItem? item)
        {
            if (item == null) return;
            if (item.Id == null)
                Items.Remove(item);
            else
                item.IsDeleted = true;
            OnPropertyChanged(nameof(Items));
        }
        private async Task SaveAsync()
        {
            bool allSuccess = true;
            foreach (var item in Items.ToList())
            {
                if (item.IsDeleted && item.Id != null)
                {
                    var success = await _service.DeleteTrainingDataAsync(item.Id.Value);
                    if (!success) allSuccess = false;
                }
                else if (item.Id == null && !string.IsNullOrWhiteSpace(item.Text))
                {
                    var success = await _service.AddTrainingDataAsync(_datasetId, item.Text, item.Sentiment);
                    if (!success) allSuccess = false;
                }
                else if (item.Id != null && !item.IsDeleted)
                {
                    var data = new TrainingData { Id = item.Id.Value, DatasetId = _datasetId, Text = item.Text, ActualSentiment = item.Sentiment };
                    var success = await _service.UpdateTrainingDataAsync(data);
                    if (!success) allSuccess = false;
                }
            }
            if (allSuccess)
                MessageBox.Show("保存成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show("部分数据保存失败！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
} 