namespace Bookstore_OOP;
using Bookstore_OOP.View;
using Bookstore_OOP.ViewModel;
using Bookstore_OOP.Services;

public partial class AdminShell : Shell
{
    //readonly DatabaseService _dbService;
    public AdminShell()
	{
        //this._dbService = dbService;
        //dbService?.InitDB();
        InitializeComponent();
        Routing.RegisterRoute("SignInPage", typeof(SignInPage));
    }

    private async void LogOutAdmin_Button(object sender, EventArgs e)
    {
        // Navigate to the login page
         await Shell.Current.GoToAsync("\\SignInPage");
        //SignInViewModel signInViewModel = new SignInViewModel(_dbService);
        //await Navigation.PushModalAsync(new SignInPage(signInViewModel));
    }
}