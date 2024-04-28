using MauiApp2.ViewModel;
namespace MauiApp2.View;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
        BindingContext = new LoginPageViewModel();

        emailEntry.Text = "user1@coventry.com";
        passwordEntry.Text = "Password123.";
    }

    
}