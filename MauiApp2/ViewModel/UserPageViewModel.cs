using MauiApp2.Model.UserDto;
using System.ComponentModel;
using System.Windows.Input;
using MauiApp2.Services; // Ensure you're using the correct namespace for ApiService_User
using System.Diagnostics;
using System.Collections.ObjectModel;
using MauiApp2.Model.ItemDto;
using MauiApp2.Model.TransactionDto;
using Microsoft.Maui.Controls; // For modal navigation
using System.Threading.Tasks;

namespace MauiApp2.ViewModel
{
    public partial class UserPageViewModel : BindableObject
    {
        private readonly ApiService_User _apiServiceUser;
        private bool _isLoading;
        private bool _isDataLoaded;

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }
        private readonly ApiService_Transaction _apiServiceTransaction;
        private TransactionHistoryDto _selectedTransaction;
        private readonly ApiService_Item _apiServiceItem;
        public TransactionHistoryDto SelectedTransaction
        {
            get => _selectedTransaction;
            set
            {
                if (value != _selectedTransaction)
                {
                    _selectedTransaction = value;
                    OnPropertyChanged();
                    
                }
            }
        }

       
        private UserDto _profile;
        public UserDto Profile
        {
            get => _profile;
            set
            {
                _profile = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoadProfileCommand { get; private set; }

        public ObservableCollection<ItemDto> MyItem { get; } = new ObservableCollection<ItemDto>();
        public ObservableCollection<TransactionHistoryDto> Transactions { get; } = new ObservableCollection<TransactionHistoryDto>();

        public UserPageViewModel()
        {
            _apiServiceUser = new ApiService_User(); 
            _apiServiceTransaction = new ApiService_Transaction(); 
            _apiServiceItem = new ApiService_Item();

            LoadProfileCommand = new Command(async () => await LoadProfileAsync());
            Transactions = new ObservableCollection<TransactionHistoryDto>();
            LoadTransactionsAsync().ConfigureAwait(false);
        }

        public async Task LoadProfileAsync()
        {
            IsLoading = true;
            Profile = await _apiServiceUser.GetUserProfileAsync();

            // Clear existing data before reloading
            MyItem.Clear();
            Transactions.Clear();

            await LoadMyItemAsync();
            await LoadTransactionsAsync();

            IsLoading = false;
        }

        private async Task LoadMyItemAsync()
        {
            var items = await _apiServiceItem.GetMyItemsForSaleAsync();
            foreach (var item in items)
            {
                MyItem.Add(item);
            }
        }

        private async Task LoadTransactionsAsync()
        {
            
            var transactions = await _apiServiceTransaction.GetUserTransactionsAsync();
            foreach (var transaction in transactions)
            {
                Transactions.Add(transaction);
            }
        }

        public async Task<bool> UpdateProfileAsync(UserUpdateDto userUpdateDto)
        {
            IsLoading = true;
            try
            {
                bool success = await _apiServiceUser.UpdateUserAsync(userUpdateDto);
                if (success)
                {
                    Profile.UserName = userUpdateDto.UserName;
                    Profile.Email = userUpdateDto.Email;
                    Profile.Address = userUpdateDto.Address;
                    Profile.SelfIntro = userUpdateDto.SelfIntro;

                    OnPropertyChanged(nameof(Profile));
                    IsLoading = false;
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception in UpdateProfileAsync: {ex.Message}");
            }
            IsLoading = false;
            return false;
        }

        public async Task<bool> DeleteAccountAsync()
        {
            return await _apiServiceUser.DeleteUserAsync();
        }

        public async Task<bool> CheckUserLoggedInAsync()
        {
            var authToken = await SecureStorage.GetAsync("auth_token");
            return !string.IsNullOrEmpty(authToken);
        }
    }
}