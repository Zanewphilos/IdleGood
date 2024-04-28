using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace MauiApp2.Model.ItemDto
{
    public class ItemDto
    {
        public int ItemId { get; set; }

        public string SellerId { get; set; }
        public List<string> ImageUrl { get; set; }

        public List<string> VideoUrl { get; set; }
        public double Price { get; set; }
        public string Title { get; set; }

        private string _adDescription;
        public string AdDescription
        {
            get => _adDescription;
            set
            {
                if (_adDescription != value)
                {
                    _adDescription = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Description { get; set; }
        public string SellerName { get; set; }
        public int LikesCount { get; set; }
        public string SellerIconUrl { get; set; }


        public string FirstImageUrl => ImageUrl?.FirstOrDefault();

        public string FirstVideoUrl => VideoUrl?.FirstOrDefault();

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}


