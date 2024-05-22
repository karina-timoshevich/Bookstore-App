using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Bookstore_OOP.View;
using Bookstore_OOP.Model;
using Bookstore_OOP.Services;
//using static Android.Content.ClipData;

namespace Bookstore_OOP.ViewModel
{
    public partial class CartViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<CartItemDisplay> _cartItems;
        DatabaseService dbService = new DatabaseService();
        [ObservableProperty]
        private CartItemDisplay _selectedCartItem;
        [ObservableProperty]
        private decimal _totalPrice;

        public CartViewModel()
        {
            dbService.InitDB();
            CartItems = new ObservableCollection<CartItemDisplay>(dbService.GetCartItems(dbService.GetCurrentUser()));
            TotalPrice = GetTotalPrice();
        }

        public decimal GetTotalPrice()
        {
            return dbService.GetTotalPrice(dbService.GetCurrentUser());
        }
        private async Task UpdateCartAndTotalPrice()
        {
            CartItems = new ObservableCollection<CartItemDisplay>(dbService.GetCartItems(dbService.GetCurrentUser()));
            TotalPrice = GetTotalPrice();
        }
        [RelayCommand]
        private async Task IncreaseQuantity()
        {
            if (_selectedCartItem != null)
            {
                await Task.Run(() => dbService.IncreaseQuantity(_selectedCartItem.Book.Id, dbService.GetCurrentUser()));
                //_selectedCartItem.Quantity++;
                CartItems = new ObservableCollection<CartItemDisplay>(dbService.GetCartItems(dbService.GetCurrentUser()));
                await UpdateCartAndTotalPrice();
            }

        }
        [RelayCommand]
        private async Task DecreaseQuantity()
        {
            if (_selectedCartItem != null)
            {
                await Task.Run(() => dbService.DecreaseQuantity(_selectedCartItem.Book.Id, dbService.GetCurrentUser()));
                // _selectedCartItem.Quantity--;
                CartItems = new ObservableCollection<CartItemDisplay>(dbService.GetCartItems(dbService.GetCurrentUser()));
                await UpdateCartAndTotalPrice();
            }

        }

        [RelayCommand]
        private async Task PlaceOrder()
        {
            await Task.Run(() => dbService.PlaceOrder(dbService.GetCurrentUser()));
            CartItems.Clear();
            TotalPrice = GetTotalPrice();
            await dbService.MakePayment(dbService.GetCurrentUser(), TotalPrice);
        }

        [RelayCommand]
        private async Task RemoveItem()
        {
            if (_selectedCartItem != null)
            {
                await Task.Run(() => dbService.RemoveItem(_selectedCartItem.Book.Id, dbService.GetCurrentUser()));
                //CartItems.Remove(_selectedCartItem);
                CartItems = new ObservableCollection<CartItemDisplay>(dbService.GetCartItems(dbService.GetCurrentUser()));
                await UpdateCartAndTotalPrice();
            }
        }
    }
}