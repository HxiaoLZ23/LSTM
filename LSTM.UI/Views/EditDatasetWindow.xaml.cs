using System.Windows;
using LSTM.UI.ViewModels;
using LSTM.Models;

namespace LSTM.UI.Views
{
    public partial class EditDatasetWindow : Window
    {
        public EditDatasetWindow(MainViewModel viewModel, Dataset dataset)
        {
            InitializeComponent();
            DataContext = viewModel;
            viewModel.EditingDataset = dataset;
            viewModel.EditDatasetName = dataset.Name;
            viewModel.EditDatasetDescription = dataset.Description;
        }
    }
} 