
using MauiApp2.View;
using System.Windows.Input;

namespace MauiApp2
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            BindingContext = this;


        }

        public static async Task EnsureUserLoggedIn()
        {
            var token = await SecureStorage.GetAsync("auth_token");
            if (string.IsNullOrEmpty(token))
            {
                // 如果用户未登录，显示登录模态页面
                await Current.Navigation.PushModalAsync(new LoginPage());
            }
        }

      
    }
}
