using MauiApp2.ViewModel;
using MauiApp2.Model.ItemDto;
using System.Windows.Input;

namespace MauiApp2.View
{
    public partial class ItemCard : ContentView
    {
        public static readonly BindableProperty ItemProperty = BindableProperty.Create(
            nameof(Item),
            typeof(ItemDto),
            typeof(ItemCard),
            propertyChanged: OnItemChanged);

        public ItemDto Item
        {
            get => (ItemDto)GetValue(ItemProperty);
            set => SetValue(ItemProperty, value);
        }

        public ItemCard()
        {
            InitializeComponent();
            // �˴�������Ҫ���� BindingContext����Ϊ���ǻ��� Item ���Ա仯ʱ��̬���������� ViewModel
        }

        private static void OnItemChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ItemCard itemCard && newValue is ItemDto newItem)
            {
                // �� Item ���Ա仯ʱ�������µ� ViewModel ������ ItemDto
                var viewModel = new ItemCardViewModel { Item = newItem };
                itemCard.BindingContext = viewModel;
            }
        }
    }
}


