using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using LSTM.Business.Interfaces;
using LSTM.Models.DTOs;
using LSTM.UI.Commands;
using Microsoft.Extensions.Logging;

namespace LSTM.UI.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<RegisterViewModel> _logger;
        private string _username = string.Empty;
        private string _email = string.Empty;
        private string _password = string.Empty;
        private string _confirmPassword = string.Empty;
        private bool _isLoading = false;
        private string _errorMessage = string.Empty;
        private string _successMessage = string.Empty;

        public RegisterViewModel(IUserService userService, ILogger<RegisterViewModel> logger)
        {
            _userService = userService;
            _logger = logger;
            
            RegisterCommand = new RelayCommand(async () => await RegisterAsync(), CanRegister);
            CancelCommand = new RelayCommand(CloseWindow);
        }

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
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

        public string SuccessMessage
        {
            get => _successMessage;
            set => SetProperty(ref _successMessage, value);
        }

        public ICommand RegisterCommand { get; }
        public ICommand CancelCommand { get; }

        private bool CanRegister()
        {
            return !IsLoading && 
                   !string.IsNullOrWhiteSpace(Username) && 
                   !string.IsNullOrWhiteSpace(Email) && 
                   !string.IsNullOrWhiteSpace(Password) && 
                   !string.IsNullOrWhiteSpace(ConfirmPassword) &&
                   Password == ConfirmPassword;
        }

        private async Task RegisterAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;
                SuccessMessage = string.Empty;

                var registerDto = new RegisterDto
                {
                    Username = Username,
                    Email = Email,
                    Password = Password,
                    ConfirmPassword = ConfirmPassword
                };

                var user = await _userService.RegisterAsync(registerDto);
                
                if (user != null)
                {
                    _logger.LogInformation("User {Username} registered successfully", Username);
                    SuccessMessage = "注册成功！请使用新账户登录。";
                    
                    // 延迟关闭窗口
                    await Task.Delay(2000);
                    CloseWindow();
                }
                else
                {
                    ErrorMessage = "注册失败，用户名或邮箱可能已存在";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Registration error");
                ErrorMessage = "注册时发生错误，请稍后重试";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void CloseWindow()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.DataContext == this)
                {
                    window.Close();
                    break;
                }
            }
        }
    }
} 