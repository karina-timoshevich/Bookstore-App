using Bookstore_OOP.Model;
using Bookstore_OOP.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace Bookstore_OOP.ViewModel
{
    public partial class UserOrdersViewModel : ObservableObject
    {
        private DatabaseService _dbService = new DatabaseService();
        [ObservableProperty]
        private ObservableCollection<Order> _orders;

        public UserOrdersViewModel()
        {
            _dbService.InitDB();
            LoadOrders();
            _orders = new ObservableCollection<Order>(_dbService.GetOrdersByUserId(_dbService.GetCurrentUser()));

            // Подпишитесь на событие OrderAdded
            _dbService.OrderAdded += LoadOrders;
        }

        private void LoadOrders()
        {
            var orders = _dbService.GetOrdersByUserId(_dbService.GetCurrentUser());
            Orders = new ObservableCollection<Order>(orders);
        }
    }
}