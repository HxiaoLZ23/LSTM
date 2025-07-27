using System.Windows;
using System.Windows.Controls;
using LSTM.UI.ViewModels;

namespace LSTM.UI.Views
{
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
            DataContext = App.GetService<RegisterViewModel>();
            
            // 处理密码框的数据绑定
            PasswordBox.PasswordChanged += (s, e) =>
            {
                if (DataContext is RegisterViewModel viewModel)
                {
                    viewModel.Password = PasswordBox.Password;
                }
            };

            ConfirmPasswordBox.PasswordChanged += (s, e) =>
            {
                if (DataContext is RegisterViewModel viewModel)
                {
                    viewModel.ConfirmPassword = ConfirmPasswordBox.Password;
                }
            };
        }
    }
} 