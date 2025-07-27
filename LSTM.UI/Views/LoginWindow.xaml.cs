using System;
using System.Windows;
using System.Windows.Controls;
using LSTM.UI.ViewModels;

namespace LSTM.UI.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            DataContext = App.GetService<LoginViewModel>();
            
            // 处理密码框的数据绑定
            PasswordBox.PasswordChanged += (s, e) =>
            {
                if (DataContext is LoginViewModel viewModel)
                {
                    viewModel.Password = PasswordBox.Password;
                }
            };
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            // 设置默认焦点到用户名输入框
            UsernameTextBox.Focus();
        }
    }
} 