using Bookstore_OOP.Model;
using Bookstore_OOP.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Bookstore_OOP.ViewModel
{
    public partial class AdminOrdersViewModel : ObservableObject
    {
        private DatabaseService _dbService = new DatabaseService();
        private ObservableCollection<Order> _orders;

        public ObservableCollection<Order> Orders
        {
            get { return _orders; }
            set
            {
                if (_orders != value)
                {
                    if (_orders != null)
                    {
                        foreach (var order in _orders)
                        {
                            order.PropertyChanged -= Order_PropertyChanged;
                        }
                    }

                    _orders = value;

                    if (_orders != null)
                    {
                        foreach (var order in _orders)
                        {
                            order.PropertyChanged += Order_PropertyChanged;
                        }
                    }
                }
            }
        }

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

        private void Order_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Order.Status))
            {
                var order = (Order)sender;
                UpdateOrderStatus(order.Id, order.Status);
            }
        }

        public void UpdateOrderStatus(int orderId, string newStatus)
        {
            _dbService.UpdateOrderStatus(orderId, newStatus);

            var order = Orders.FirstOrDefault(o => o.Id == orderId);
            if (order != null)
            {
                order.Status = newStatus;
            }
        }
    }
}