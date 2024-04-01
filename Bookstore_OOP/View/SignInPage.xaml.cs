namespace Bookstore_OOP.View;
using Bookstore_OOP.Services;
using Bookstore_OOP.ViewModel;
public partial class SignInPage : ContentPage
{
    readonly SignInViewModel _signInViewModel;
    readonly IServiceProvider _serviceProvider;
    public SignInPage(SignInViewModel signInViewModel, IServiceProvider serviceProvider)
    {
        this._signInViewModel = signInViewModel;
        InitializeComponent();
        _serviceProvider = serviceProvider;
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
                    //await Shell.Current.GoToAsync("//UserMainPage");
                    Application.Current.MainPage = new UserShell(_serviceProvider);
                }
                //await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Ошибка", "Неверный email или пароль", "OK");
            }
        }
    }
}