using MauiApp2.ViewModel;

namespace MauiApp2.View;

public partial class RegisterPage : ContentPage
{
    public RegisterPage()
    {
        InitializeComponent();

        // …Ë÷√ ViewModel
        BindingContext = new RegisterPageViewModel();
    }
}