using MauiApp2.ViewModel;
using MauiApp2.Model.TransactionDto;


namespace MauiApp2.View;

public partial class TransactionDetailPage : ContentPage
{
    private TransactionDetailPageViewModel _viewModel;
    public TransactionDetailPage(TransactionHistoryDto transaction)
    {
        InitializeComponent();

        // ���� ViewModel ������ transaction
        _viewModel = new TransactionDetailPageViewModel(transaction);
        this.BindingContext = _viewModel;

        _viewModel.ClosePageRequested += async (s, e) => await ClosePageAsync(); // Ϊ ViewModel �� ClosePageRequested �¼���Ӵ������
        _viewModel.RequestUpdateStatus += async (s, e) =>
        {
            var action = await DisplayActionSheet("Update Status", "Cancel", null, "In Progress", "Completed");
            if (action == "In Progress" || action == "Completed")
            {
                
                var statusUpdateDto = new TransactionStatusUpdateDto
                {
                    
                    Status = action
                };
                await _viewModel.UpdateTransactionStatusAsync(transaction.TransactionId, statusUpdateDto);
            }
        };
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        // �����ģ̬ҳ��
        await Navigation.PopModalAsync();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.ErrorOccurred += ViewModel_ErrorOccurred;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _viewModel.ErrorOccurred -= ViewModel_ErrorOccurred;
        _viewModel.ClosePageRequested -= async (s, e) => await ClosePageAsync();
        
    }
    private async Task ClosePageAsync()
    {
        await Navigation.PopModalAsync();
 
    }

    private async void ViewModel_ErrorOccurred(object sender, string e)
    {
        await DisplayAlert("Error", e, "OK");
    }
}