namespace Bookstore_OOP.View;

//using AndroidX.Lifecycle;
using Bookstore_OOP.Model;
using Bookstore_OOP.Services;
using Bookstore_OOP.ViewModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

public partial class UserOrders : ContentPage
{
   private UserOrdersViewModel _userOrdersViewModel;
    public UserOrders()
    {
        
        InitializeComponent();
        _userOrdersViewModel = new UserOrdersViewModel();
        BindingContext = _userOrdersViewModel;
    }

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // Получите выбранный заказ
        var selectedOrder = (Order)e.CurrentSelection.FirstOrDefault();

        if (selectedOrder != null)
        {
            // Перейдите на новую страницу, которая отображает содержимое выбранного заказа
            // Navigation.PushAsync(new OrderDetailsPage(selectedOrder));
        }
    }
}