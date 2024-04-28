using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MauiApp2.View;
using System.Diagnostics;
using MauiApp2.Model.ItemDto;

namespace MauiApp2.ViewModel
{
    public class ItemCardViewModel : BindableObject
    {

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

        public ICommand OpenItemDetailCommand { get; }

        public event Action<ItemDto> RequestNavigate;

        public ItemCardViewModel()
        {
            
            OpenItemDetailCommand = new Command(ExecuteOpenItemDetailCommand);
        }

        private async void ExecuteOpenItemDetailCommand()
        {
            if (Item != null)
            {
                // Use MainPage's Navigation property to push the new page
                await Application.Current.MainPage.Navigation.PushModalAsync(new ItemDetailPage(Item));
            }
        }




    }
}
