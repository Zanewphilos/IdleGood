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
            // 此处不再需要设置 BindingContext，因为我们会在 Item 属性变化时动态创建并设置 ViewModel
        }

        private static void OnItemChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ItemCard itemCard && newValue is ItemDto newItem)
            {
                // 当 Item 属性变化时，创建新的 ViewModel 并传递 ItemDto
                var viewModel = new ItemCardViewModel { Item = newItem };
                itemCard.BindingContext = viewModel;
            }
        }
    }
}


