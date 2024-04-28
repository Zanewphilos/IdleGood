using System.Windows.Input;
using Microsoft.Maui.Controls;
using MauiApp2.Services; // 引入服务命名空间
using System.Threading.Tasks;

namespace MauiApp2.ViewModel
{
    public class RegisterPageViewModel : BindableObject
    {
        private readonly ApiService_User _apiService = new ApiService_User();

        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public ICommand RegisterCommand { get; }
        public ICommand NavigateBackCommand { get; }

        public RegisterPageViewModel()
        {
            RegisterCommand = new Command(async () => await OnRegisterClicked());
            NavigateBackCommand = new Command(async () => await OnBackToLoginClicked());
        }

        private async Task OnRegisterClicked()
        {
            if (Password != ConfirmPassword)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Passwords do not match.", "OK");
                return;
            }

           // call the register method from ApiService
            bool isRegistrationSuccessful = await _apiService.RegisterAsync(Username, Email, Password);
            if (isRegistrationSuccessful)
            {
                await Shell.Current.Navigation.PopModalAsync();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Registration Failed", "An error occurred during registration.", "OK");
            }
        }

        private async Task OnBackToLoginClicked()
        {
            await Shell.Current.Navigation.PopModalAsync();
        }
    }
}
