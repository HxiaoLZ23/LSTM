using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using LSTM.Business.Interfaces;
using LSTM.Models.DTOs;
using LSTM.UI.Commands;
using LSTM.UI.Views;
using Microsoft.Extensions.Logging;

namespace LSTM.UI.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<LoginViewModel> _logger;
        private string _username = string.Empty;
        private string _password = string.Empty;
        private bool _isLoading = false;
        private string _errorMessage = string.Empty;

        public LoginViewModel(IUserService userService, ILogger<LoginViewModel> logger)
        {
            _userService = userService;
            _logger = logger;
            
            LoginCommand = new RelayCommand(async () => await LoginAsync(), () => !IsLoading && !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password));
            RegisterCommand = new RelayCommand(OpenRegisterWindow);
            ExitCommand = new RelayCommand(() => Application.Current.Shutdown());
        }

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }
        public ICommand ExitCommand { get; }

        private async Task LoginAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;

                var loginDto = new LoginDto
                {
                    Username = Username,
                    Password = Password
                };

                var user = await _userService.AuthenticateAsync(loginDto);
                
                if (user != null)
                {
                    _logger.LogInformation("User {Username} logged in successfully", Username);
                    
                    // 打开主窗口
                    var mainWindow = App.GetService<MainWindow>();
                    var mainViewModel = App.GetService<MainViewModel>();
                    mainViewModel.CurrentUser = user;
                    mainWindow.DataContext = mainViewModel;
                    mainWindow.Show();
                    
                    // 关闭登录窗口
                    if (Application.Current.MainWindow is LoginWindow loginWindow)
                    {
                        loginWindow.Close();
                    }
                }
                else
                {
                    ErrorMessage = "用户名或密码错误";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login error");
                ErrorMessage = "登录时发生错误，请稍后重试";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void OpenRegisterWindow()
        {
            var registerWindow = App.GetService<RegisterWindow>();
            registerWindow.Owner = Application.Current.MainWindow;
            registerWindow.ShowDialog();
        }
    }
} 