using System.Windows.Input;
using MauiApp2.View;
using Microsoft.Maui.Controls;
using MauiApp2.Services;
using System.Diagnostics;

namespace MauiApp2.ViewModel
{
    public class LoginPageViewModel : BindableObject
    {
        private readonly ApiService_User _apiService = new ApiService_User();

       

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }

        public ICommand SkipLoginCommand { get; }

        public LoginPageViewModel()
        {
            LoginCommand = new Command(async () => await OnLoginClicked());
            RegisterCommand = new Command(async () => await OnRegisterClicked());
            SkipLoginCommand = new Command(async () => await OnSkipLoginClicked());
        }


        private async Task OnLoginClicked()
        {
            Debug.WriteLine($"Email: {Email}, Password: {Password}");
            var (isLoginSuccessful, errorMessage) = await _apiService.LoginAsync(Email, Password);
            if (isLoginSuccessful)
            {
                await Shell.Current.GoToAsync("//home"); 
                Debug.WriteLine("Login successful");
            }
            else
            {
                // 显示错误消息
                await Application.Current.MainPage.DisplayAlert("Login Failed", errorMessage, "OK");
            }
        }

        private async Task OnRegisterClicked()
        {
            
            await Application.Current.MainPage.Navigation.PushModalAsync(new RegisterPage());
        }

        private async Task OnSkipLoginClicked()
        {
            // 跳过登录，导航到应用的主界面
            await Shell.Current.GoToAsync("//home");
        }
    }
}