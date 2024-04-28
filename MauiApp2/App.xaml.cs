
using MauiApp2.View;

namespace MauiApp2
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
            CheckDisplayLogin();
        }

        private async void CheckDisplayLogin()
        {
            await MainPage.Navigation.PushModalAsync(new LoginPage());
            //    var token = await SecureStorage.GetAsync("auth_token");
            //    if (string.IsNullOrEmpty(token))
            //    {
            //        // 如果没有 token，以模态形式显示登录页面
            //        await MainPage.Navigation.PushModalAsync(new LoginPage());
            //    }
            //    // 如果有 token，应用将直接显示 AppShell 定义的主界面
            //
        }
    }
}
