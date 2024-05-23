using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookstore_OOP.Model;
using Bookstore_OOP.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace Bookstore_OOP.ViewModel
{
    public partial class OrderDetailsViewModel : ObservableObject
    {
        private DatabaseService _dbService = new DatabaseService();
        [ObservableProperty]
        private Order _selectedOrder;
        [ObservableProperty]
        private ObservableCollection<Book> _booksInOrder;

        public OrderDetailsViewModel(Order selectedOrder)
        {
            _selectedOrder = _dbService.GetOrderDetails(selectedOrder.Id);
            if (_selectedOrder != null && _selectedOrder.Books != null)
            {
                BooksInOrder = new ObservableCollection<Book>(_selectedOrder.Books);
            }
            else
            {
                BooksInOrder = new ObservableCollection<Book>();
            }
        }
    }
}
