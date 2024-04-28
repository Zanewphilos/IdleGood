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

        // ����û��Ƿ��¼
        if (!await IsUserLoggedIn())
        {
            // ��ʾ��¼ģ̬ҳ��
            await Navigation.PushModalAsync(new LoginPage());
        }
    }

    private async Task<bool> IsUserLoggedIn()
    {
        // ���첽��ʽ�Ӱ�ȫ�洢�л�ȡ����
        var authToken = await SecureStorage.GetAsync("auth_token");
       
        return !string.IsNullOrEmpty(authToken);
    }
}