using System.Windows.Input;
using MauiApp2.Model.TransactionDto;
using MauiApp2.View; 
using Microsoft.Maui.Controls; 
using System.Threading.Tasks;
using System.Diagnostics;

namespace MauiApp2.ViewModel
{
    public class TransactionCardViewModel : BindableObject
    {
        private TransactionHistoryDto _transaction;
        public TransactionHistoryDto Transaction
        {
            get => _transaction;
            set
            {
                _transaction = value;
                OnPropertyChanged(nameof(Transaction));
            }
        }

        public ICommand TransactionSelectedCommand { get; }

        public TransactionCardViewModel()
        {
            
            TransactionSelectedCommand = new Command(async () => await OpenTransactionDetailPageAsync());
        }

        private async Task OpenTransactionDetailPageAsync()
        {
            // Check if Transaction is null to avoid opening a detail page with no data
            if (Transaction == null)
            {
                Debug.WriteLine("Transaction data is missing.");
                return;
            }

            
            var detailPage = new TransactionDetailPage(Transaction);

            await Application.Current.MainPage.Navigation.PushModalAsync(detailPage);
        }
    }
}