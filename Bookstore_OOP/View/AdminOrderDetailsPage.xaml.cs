using Bookstore_OOP.ViewModel;
using Bookstore_OOP.Model;
using System;
using Microsoft.Maui.Controls;

namespace Bookstore_OOP.View
{
    public partial class AdminOrderDetailsPage : ContentPage
    {
        private AdminOrderDetailsViewModel _adminOrderDetailsViewModel;

        public AdminOrderDetailsPage(Order selectedOrder)
        {
            InitializeComponent();
            _adminOrderDetailsViewModel = new AdminOrderDetailsViewModel(selectedOrder);
            BindingContext = _adminOrderDetailsViewModel;
        }
    }
}