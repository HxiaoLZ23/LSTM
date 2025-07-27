using System.Windows;
using LSTM.UI.ViewModels;
using LSTM.Business.Interfaces;

namespace LSTM.UI.Views
{
    public partial class EditDatasetContentWindow : Window
    {
        public EditDatasetContentWindow(IModelTrainingService service, int datasetId)
        {
            InitializeComponent();
            var vm = new EditDatasetContentViewModel(service, datasetId);
            DataContext = vm;
            Loaded += async (s, e) => await vm.LoadAsync();
        }
    }
} 