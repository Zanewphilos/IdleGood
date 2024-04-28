using MauiApp2.ViewModel;

namespace MauiApp2.View
{
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
            BindingContext = new HomeViewModel();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var viewModel = BindingContext as HomeViewModel;
            if (viewModel != null)
            {
                await viewModel.RefreshItemsAsync();
            }
        }
    }
    
}