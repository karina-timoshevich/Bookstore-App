using Bookstore_OOP.ViewModel;
using Bookstore_OOP.Model;
using Bookstore_OOP.Services;
using System;
using Bookstore_OOP.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Bookstore_OOP.View;

public partial class AdminOrders : ContentPage
{

    private AdminOrdersViewModel _adminOrdersViewModel;
    public AdminOrders()
    {

        InitializeComponent();
        _adminOrdersViewModel = new AdminOrdersViewModel();
        BindingContext = _adminOrdersViewModel;

    }
  
}