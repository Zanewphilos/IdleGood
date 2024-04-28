using MauiApp2.ViewModel;
using MauiApp2.Model.TransactionDto;

namespace MauiApp2.View;

public partial class TransactionCard : ContentView
{
    public static readonly BindableProperty TransactionProperty = BindableProperty.Create(
        nameof(Transaction),
        typeof(TransactionHistoryDto),
        typeof(TransactionCard),
        propertyChanged: OnTransactionChanged);

    public TransactionHistoryDto Transaction
    {
        get => (TransactionHistoryDto)GetValue(TransactionProperty);
        set => SetValue(TransactionProperty, value);
    }

    public TransactionCard()
    {
        InitializeComponent();
    }

    private static void OnTransactionChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TransactionCard card && newValue is TransactionHistoryDto transaction)
        {
            
            var viewModel = new TransactionCardViewModel
            {
                Transaction = transaction
            };
            card.BindingContext = viewModel;
        }
    }
}