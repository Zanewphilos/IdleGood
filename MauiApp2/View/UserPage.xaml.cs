using MauiApp2.Model.UserDto;
using MauiApp2.ViewModel;
using Microsoft.Maui.Controls;

namespace MauiApp2.View;



[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class UserPage : ContentPage
{
    private UserPageViewModel _viewModel;

    public UserPage()
    {
        InitializeComponent();
        _viewModel = new UserPageViewModel();
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var isLoggedIn = await _viewModel.CheckUserLoggedInAsync();

        if (!isLoggedIn)
        {
            await Navigation.PushModalAsync(new LoginPage(), animated: true);
        }
        else
        {
            await _viewModel.LoadProfileAsync();
        }
    }

    private async Task<bool> IsUserLoggedIn()
    {
        // 以异步方式从安全存储中获取令牌
        var authToken = await SecureStorage.GetAsync("auth_token");
        // 根据令牌是否存在来返回登录状态，这里还可以加上令牌有效性的检查
        return !string.IsNullOrEmpty(authToken);
    }

    private void OnEditProfileClicked(object sender, EventArgs e)
    {
        if (!EditProfileForm.IsVisible)
        {
            // when the form is shown, populate the fields with the current user profile
            NameEntry.Text = _viewModel.Profile.UserName;
            EmailEntry.Text = _viewModel.Profile.Email;
            AddressEntry.Text = _viewModel.Profile.Address;
            SelfIntroEditor.Text = _viewModel.Profile.SelfIntro;
        }

        EditProfileForm.IsVisible = !EditProfileForm.IsVisible;
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        // 确认是否登出
        bool confirm = await DisplayAlert("Logout", "Are you sure you want to logout?", "Yes", "No");
        if (confirm)
        {
            SecureStorage.Remove("auth_token");
            await Navigation.PushModalAsync(new LoginPage());
        }
    }

    private async void OnDeleteAccountClicked(object sender, EventArgs e)
    {
        // 确认是否注销账号
        bool confirm = await DisplayAlert("Cancellation of Account", "This will permanently delete your account, are you sure you want to continue?", "Yes", "No");
        if (confirm)
        {
            bool success = await _viewModel.DeleteAccountAsync();
            if (success)
            {
                await DisplayAlert("Success", "Your account has been successfully deleted.", "OK");
                SecureStorage.Remove("auth_token"); // 清除认证信息

                // 跳转到欢迎或登录页面，并清空导航堆栈
                await Navigation.PushModalAsync(new LoginPage());
                
            }
            else
            {
                await DisplayAlert("Error", "Unable to delete account, please try again later.", "OK");
            }
        }
    }

    private async void OnSaveProfileClicked(object sender, EventArgs e)
    {
        var updatedProfile = new UserUpdateDto
        {
            UserName = NameEntry.Text,
            Email = EmailEntry.Text,
            Address = AddressEntry.Text,
            SelfIntro = SelfIntroEditor.Text
        };

        // 调用ViewModel中的更新方法
        bool updateResult = await _viewModel.UpdateProfileAsync(updatedProfile);

        if (updateResult)
        {
            // 更新视图模型中的用户信息（如果需要）
            _viewModel.Profile.UserName = updatedProfile.UserName;
            _viewModel.Profile.Email = updatedProfile.Email;
            _viewModel.Profile.Address = updatedProfile.Address;
            _viewModel.Profile.SelfIntro = updatedProfile.SelfIntro;

            // 通知用户更新成功
            await DisplayAlert("Success", "Your personal information has been updated.", "OK");

            // 隐藏编辑表单
            EditProfileForm.IsVisible = false;
        }
        else
        {
            // 更新失败，通知用户
            await DisplayAlert("Error", "Failed to update personal information, please try again later.", "OK");
        }
    }


}