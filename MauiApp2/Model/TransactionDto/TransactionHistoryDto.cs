using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MauiApp2.Model.TransactionDto
{
    public class TransactionHistoryDto
    {
        public int TransactionId { get; set; }
        public string? ItemTitle { get; set; } // 假设你想传输商品的标题
        public List<string>? ItemUrl { get; set; }
        public string? BuyerName { get; set; }

        public string? BuyerId { get; set; }
        public string? SellerName { get; set; }
        public string? SellerId { get; set; }

        private string _TransactionStatus;
        public string TransactionStatus
        {
            get => _TransactionStatus;
            set
            {
                if (_TransactionStatus != value)
                {
                    _TransactionStatus = value;
                    OnPropertyChanged();
                }
            }
        }
        public DateTime TransactionDate { get; set; }

        public string FirstImageUrl => ItemUrl?.FirstOrDefault();

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
