namespace Bookstore_OOP.View;
using Bookstore_OOP.ViewModel;

public partial class OrderDetailsPage : ContentPage
{
    private OrderDetailsViewModel _orderDetailsViewModel;

    public OrderDetailsPage(OrderDetailsViewModel viewModel)
    {
        InitializeComponent();
        _orderDetailsViewModel = viewModel;
        BindingContext = _orderDetailsViewModel;
    }
}