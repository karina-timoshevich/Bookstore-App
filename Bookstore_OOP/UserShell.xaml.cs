namespace Bookstore_OOP.View;

public partial class UserShell : Shell
{
    private readonly IServiceProvider _serviceProvider;
    public UserShell(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        InitializeComponent();
        Routing.RegisterRoute("SignInPage", typeof(SignInPage));
    }

    private async void LogOut_Button(object sender, EventArgs e)
    {
        var signInPage = _serviceProvider.GetRequiredService<SignInPage>();
        // await Navigation.PushModalAsync(signInPage);
        await Shell.Current.GoToAsync("SignInPage");
        Application.Current.MainPage = new AppShell();
    }
}