using Bookstore_OOP.Model;
using Bookstore_OOP.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace Bookstore_OOP.ViewModel
{
    public partial class AdminOrdersViewModel : ObservableObject
    {
        private DatabaseService _dbService = new DatabaseService();
        [ObservableProperty]
        private ObservableCollection<Order> _orders;

        public AdminOrdersViewModel()
        {
            _dbService.InitDB();
            LoadOrders();

            // Subscribe to the OrderAdded event
            _dbService.OrderAdded += LoadOrders;
        }

        private void LoadOrders()
        {
            var orders = _dbService.GetAllOrders();
            Orders = new ObservableCollection<Order>(orders);
        }

        public void UpdateOrderStatus(int orderId, string newStatus)
        {
            _dbService.UpdateOrderStatus(orderId, newStatus);
            LoadOrders();
        }
    }
}