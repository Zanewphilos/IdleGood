using System.ComponentModel;
using System.Runtime.CompilerServices;
using MauiApp2.Model.ItemDto;
using MauiApp2.Model.TransactionDto;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Storage;
using MauiApp2.Services;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using MauiApp2.Model;

namespace MauiApp2.ViewModel{
    public class ItemDetailViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly ApiService_Item _apiService_Item;

        private readonly ApiService_Transaction _apiService_Transaction;

        private ItemDto _item;
        public ItemDto Item
        {
            get => _item;
            set
            {
                _item = value;
                OnPropertyChanged();
            }
        }


        // Commands
        public ICommand BuyItemCommand { get; }
        public ICommand AddDescriptionCommand { get; }
        public ICommand DeleteItemCommand { get; }

        public event EventHandler<string> BuyItemSuccess;
        public event EventHandler<string> BuyItemFailed;
        public event EventHandler RequestAddDescription;
        public event EventHandler<string> ErrorOccurred;
        public event EventHandler ItemDeleted;
        public ItemDetailViewModel(ItemDto item)
        {
            Item = item;
            Console.WriteLine(item.ItemId);
            _apiService_Item = new ApiService_Item();
            _apiService_Transaction = new ApiService_Transaction();

            
            // Initialize commands
            BuyItemCommand = new Command(async () => await BuyItemAsync());
            AddDescriptionCommand = new Command(() => OnAddDescriptionRequested());
            DeleteItemCommand = new Command(async () => await DeleteItemAsync());
        }

       
        private async Task BuyItemAsync()
        {
            try
            {
                var transactionDto = new TransactionCreateDto
                {
                    ItemId = this.Item.ItemId
                };
                var success = await _apiService_Transaction.PostTransactionAsync(transactionDto);
                if (success)
                {
                    Console.WriteLine("Purchase successful.");

                    BuyItemSuccess?.Invoke(this, "Purchase successful.");
                    await Shell.Current.GoToAsync("//home");
                }
                else
                {
                    Console.WriteLine("Purchase failed.");
                    ErrorOccurred?.Invoke(this, "Failed to complete the purchase. Please try again.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error buying item: {ex.Message}");
                ErrorOccurred?.Invoke(this, ex.Message);
            }


        }

        public async Task AddDescriptionAsync(string newDescription)
        {
            if (!await IsCurrentUserTheSellerAsync())
            {
                ErrorOccurred?.Invoke(this, "You are not the Seller");
                return;
            }
            

            if (string.IsNullOrEmpty(newDescription))
            {
                ErrorOccurred?.Invoke(this, "Description cannot be empty.");
                return;
            }

            try
            {
                var success = await _apiService_Item.UpdateItemDescriptionAsync(Item.ItemId, new ItemUpdateDto { AdDescription = newDescription });
                if (success)
                {
                    Console.WriteLine("Description updated successfully.");
                    Item.AdDescription = newDescription;
                    OnPropertyChanged(nameof(Item));
                }
                else
                {
                    Console.WriteLine("Failed to update description.");
                    ErrorOccurred?.Invoke(this, "Failed to update description. Please try again.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating description: {ex.Message}");
                ErrorOccurred?.Invoke(this, ex.Message);
            }
        }
        public void OnAddDescriptionRequested()
        {
            RequestAddDescription?.Invoke(this, EventArgs.Empty);
        }
        private async Task DeleteItemAsync()
        {
            if (!await IsCurrentUserTheSellerAsync())
            {
                ErrorOccurred?.Invoke(this, "You are not the Seller");
                return;
            }

            try
            {
                var success = await _apiService_Item.DeleteItemAsync(Item.ItemId);
                if (success)
                {
                    Console.WriteLine("Item deleted successfully.");
                    ItemDeleted?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    Console.WriteLine("Failed to delete item.");
                    ErrorOccurred?.Invoke(this, "Failed to delete item. Please try again.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting item: {ex.Message}");
                ErrorOccurred?.Invoke(this, ex.Message);
            }
        }

        private async Task<bool> IsCurrentUserTheSellerAsync()
        {
            var userId = await SecureStorage.GetAsync("user_id");
            return Item.SellerId.ToString() == userId;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
