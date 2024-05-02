namespace Bookstore_OOP.View;
using Bookstore_OOP.Services;
using Bookstore_OOP.ViewModel;
using Xamarin.Essentials;
//using Microsoft.Maui.Essentials
public partial class SignInPage : ContentPage
{
    readonly SignInViewModel _signInViewModel;
    readonly IServiceProvider _serviceProvider;
    DatabaseService _dbService = new DatabaseService();
    public SignInPage(SignInViewModel signInViewModel, IServiceProvider serviceProvider)
    {
        this._signInViewModel = signInViewModel;
        InitializeComponent();
        _serviceProvider = serviceProvider;
        _dbService.InitDB();
    }
    private async void TapGestureRecognizer_Tapped_For_SignUP(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//SignUp");
    }

    private async void Sign_In_Button(object sender, EventArgs e)
    {
        string email = Email_Entry_SignIn.Text;
        string password = Password_Entry_SignIn.Text;
        if (email == null || password == null)
        {
           await DisplayAlert("Ошибка", "Заполните все поля", "OK");
            return;
        }
        else
        {
            if (_signInViewModel.SignInUser(email, password))
            {
                if (email == "admin" && password == "admin")
                {
                    //await Shell.Current.GoToAsync("//AdminMainPage");
                    Application.Current.MainPage = new AdminShell(_serviceProvider);
                }
                else
                {
                    int userId = _dbService.GetUserIdByEmail(email);
                    _dbService.SetCurrentUser(userId);


                    //Microsoft.Maui.Essentials.Preferences.Set("CurrentUserID", userId);
                    //await Shell.Current.GoToAsync("//UserMainPage");
                    Application.Current.MainPage = new UserShell();
                }
            }
            else
            {
                await DisplayAlert("Ошибка", "Неверный email или пароль", "OK");
            }
        }
    }
}