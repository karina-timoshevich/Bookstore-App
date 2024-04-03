namespace Bookstore_OOP;
using Bookstore_OOP.View;
using Bookstore_OOP.ViewModel;
using Bookstore_OOP.Services;

public partial class AdminShell : Shell
{
    private readonly IServiceProvider _serviceProvider;

    public AdminShell(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        Routing.RegisterRoute("SignInPage", typeof(SignInPage));
        Routing.RegisterRoute("AddUserView", typeof(AddUserView));
    }

    private async void LogOutAdmin_Button(object sender, EventArgs e)
    {
        var signInPage = _serviceProvider.GetRequiredService<SignInPage>();
       // await Navigation.PushModalAsync(signInPage);
        await Shell.Current.GoToAsync("SignInPage");
        Application.Current.MainPage = new AppShell();

    }
}