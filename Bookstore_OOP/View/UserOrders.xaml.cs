namespace Bookstore_OOP.View;

//using AndroidX.Lifecycle;
using Bookstore_OOP.Model;
using Bookstore_OOP.Services;
using Bookstore_OOP.ViewModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _userOrdersViewModel.LoadOrders();
    }

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedOrder = (Order)e.CurrentSelection.FirstOrDefault();

        if (selectedOrder != null)
        {
            Debug.WriteLine("\n" + selectedOrder.Id + "\n");
            Navigation.PushAsync(new OrderDetailsPage(new OrderDetailsViewModel(selectedOrder)));
        }
    }
}