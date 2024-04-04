namespace Bookstore_OOP.View;
using Bookstore_OOP.ViewModel;

public partial class CartView : ContentPage
{
    private CartViewModel _cartViewModel;
    public CartView()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        _cartViewModel = new();
        BindingContext = _cartViewModel;

        base.OnNavigatedTo(args);
    }
}