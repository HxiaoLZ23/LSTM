using System;
using System.Windows;
using LSTM.UI.ViewModels;
using LSTM.Models;

namespace LSTM.UI.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        protected override async void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            
            // 初始化数据
            if (DataContext is MainViewModel viewModel)
            {
                await viewModel.InitializeAsync();
            }
        }

        public void ShowEditDatasetDialog(Dataset dataset)
        {
            if (DataContext is MainViewModel viewModel)
            {
                var dialog = new EditDatasetWindow(viewModel, dataset)
                {
                    Owner = this
                };
                dialog.ShowDialog();
            }
        }
    }
} 