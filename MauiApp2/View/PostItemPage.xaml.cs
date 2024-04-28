using MauiApp2.ViewModel;

namespace MauiApp2.View;

    public partial class PostItemPage : ContentPage
    {
        public PostItemPage()
        {
            InitializeComponent();
            BindingContext = new PostItemViewModel();
        }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // 检查用户是否登录
        if (!await IsUserLoggedIn())
        {
            // 显示登录模态页面
            await Navigation.PushModalAsync(new LoginPage());
        }
    }

    private async Task<bool> IsUserLoggedIn()
    {
        // 以异步方式从安全存储中获取令牌
        var authToken = await SecureStorage.GetAsync("auth_token");
       
        return !string.IsNullOrEmpty(authToken);
    }
}