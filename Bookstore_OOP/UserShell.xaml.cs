namespace Bookstore_OOP.View;
using Xamarin.Essentials;

public partial class UserShell : Shell
{
   // private readonly IServiceProvider _serviceProvider;
    public UserShell()
    {
        //_serviceProvider = serviceProvider;
        InitializeComponent();
        Routing.RegisterRoute("SignInPage", typeof(SignInPage));
        Routing.RegisterRoute("TestPayment", typeof(TestPayment));
        Routing.RegisterRoute("UserOrders", typeof(UserOrders));
        Routing.RegisterRoute("UserCart", typeof(CartView));
        Routing.RegisterRoute("OrderDetailsPage", typeof(OrderDetailsPage));
    }

    private async void LogOut_Button(object sender, EventArgs e)
    {
        //var signInPage = _serviceProvider.GetRequiredService<SignInPage>();


        await Shell.Current.GoToAsync("SignInPage");
        Application.Current.MainPage = new AppShell();
    }
}