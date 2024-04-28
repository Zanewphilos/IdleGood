using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using MauiApp2.Model.ItemDto;
using MauiApp2.Services;
using MauiApp2.View;

namespace MauiApp2.ViewModel
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        private bool _isLoading;//for loading animation

        private bool _isDataLoaded;//for load data only once
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<ItemDto> ItemsYouMightLike { get; } = new ObservableCollection<ItemDto>();
        public ObservableCollection<ItemDto> PopularItems { get; } = new ObservableCollection<ItemDto>();
        public ObservableCollection<ItemDto> AllItem { get; } = new ObservableCollection<ItemDto>();

        public ICommand NavigateToItemDetailCommand { get; }
        // 实现 INotifyPropertyChanged 接口的事件
        public event PropertyChangedEventHandler PropertyChanged;

        public HomeViewModel()
        {
            NavigateToItemDetailCommand = new Command<ItemDto>(NavigateToItemDetail);
            _isDataLoaded = false;
        }
        private async void NavigateToItemDetail(ItemDto item)
        {
            Debug.WriteLine("NavigateToItemDetail");
            if (item != null)
            {
                await Application.Current.MainPage.Navigation.PushModalAsync(new ItemDetailPage(item));
            }
        }
        public async Task InitializeAsync()
        {
            if (!_isDataLoaded) 
            {
                IsLoading = true;
                await LoadItemsYouMightLikeAsync();
                await LoadPopularItemsAsync();
                await LoadAllItemsAsync();
                IsLoading = false;
                _isDataLoaded = true; 
            }
        }

        public async Task RefreshItemsAsync()
        {
            IsLoading = true;

            ItemsYouMightLike.Clear();
            PopularItems.Clear();
            AllItem.Clear();

            await LoadItemsYouMightLikeAsync();
            await LoadPopularItemsAsync();
            await LoadAllItemsAsync();

            IsLoading = false;
        }

        private async Task LoadItemsYouMightLikeAsync()
        {
            ApiService_Item apiService = new ApiService_Item();
            var items = await apiService.GetItemsYouMightLikeAsync();
            Debug.WriteLine(items);
            foreach (var item in items)
            {
                Debug.WriteLine(item);
                ItemsYouMightLike.Add(item);
            }
        }

        private async Task LoadPopularItemsAsync()
        {
            ApiService_Item apiService = new ApiService_Item();
            var items = await apiService.GetItemsPopularAsync();
            Debug.WriteLine(items);
            foreach (var item in items)
            {
               
                PopularItems.Add(item);
            }
        }

        private async Task LoadAllItemsAsync()
        {
            ApiService_Item apiService = new ApiService_Item();
            var items = await apiService.GetItemsAllAsync();
            Debug.WriteLine(items);
            foreach (var item in items)
            {
                
                AllItem.Add(item);
            }
        }

   


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
