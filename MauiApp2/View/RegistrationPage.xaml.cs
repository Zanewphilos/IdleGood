using MauiApp2.ViewModel;

namespace MauiApp2.View;

public partial class RegisterPage : ContentPage
{
    public RegisterPage()
    {
        InitializeComponent();

        // ���� ViewModel
        BindingContext = new RegisterPageViewModel();
    }
}