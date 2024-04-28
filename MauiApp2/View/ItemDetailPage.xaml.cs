using MauiApp2.Model.ItemDto;
using MauiApp2.ViewModel;
using Microsoft.Maui.Controls;
using System;

namespace MauiApp2.View
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage(ItemDto item)
        {
            InitializeComponent();
            var viewModel = new ItemDetailViewModel(item);
            Console.WriteLine("ItemDetailPage: " + item.Description);
            BindingContext = viewModel;
            viewModel.BuyItemSuccess += ViewModel_BuyItemSuccess;
            viewModel.BuyItemFailed += ViewModel_BuyItemFailed;
            viewModel.ErrorOccurred += ViewModel_ErrorOccurred;
            viewModel.RequestAddDescription += async (s, e) =>
            {
                var newDescription = await DisplayPromptAsync("Add Description", "Enter the new description:");
                if (!string.IsNullOrEmpty(newDescription))
                {
                    
                    await viewModel.AddDescriptionAsync(newDescription);
                }
            };
            viewModel.ItemDeleted += async (s, e) =>
            {
                // when delete item, pop the page
                await Navigation.PopModalAsync();
            };

        }
        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            // �����ģ̬ҳ��
            await Navigation.PopModalAsync();
        }

        private async void ViewModel_ErrorOccurred(object sender, string e)
        {
            await DisplayAlert("Error", e, "OK");
        }

        private async void ViewModel_BuyItemSuccess(object sender, string e)
        {
            await DisplayAlert("Success", e, "OK");
            // ���������������ɹ���Ĳ���
            await Navigation.PopModalAsync();
        }

        private async void ViewModel_BuyItemFailed(object sender, string e)
        {
            await DisplayAlert("Failed", e, "OK");
            // ��������������ʧ�ܺ�Ĳ���
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (BindingContext is ItemDetailViewModel viewModel)
            {
                // ȷ����ҳ����ʧʱȡ�����������¼�
                viewModel.BuyItemSuccess -= ViewModel_BuyItemSuccess;
                viewModel.BuyItemFailed -= ViewModel_BuyItemFailed;
                viewModel.ErrorOccurred -= ViewModel_ErrorOccurred;
                viewModel.ItemDeleted -= async (s, e) => await Navigation.PopModalAsync();
                //cancel the mediaElement's handler
                mediaElement.Handler?.DisconnectHandler();
            }
        }
    }
}