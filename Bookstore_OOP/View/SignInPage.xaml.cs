namespace Bookstore_OOP.View;
using Bookstore_OOP.Services;
using Bookstore_OOP.ViewModel;
public partial class SignInPage : ContentPage
{
    readonly SignInViewModel _signInViewModel;

    public SignInPage(SignInViewModel signInViewModel)
    {
        this._signInViewModel = signInViewModel;
        InitializeComponent();
    }
    private async void TapGestureRecognizer_Tapped_For_SignUP(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//SignUp");
    }

    private void Sign_In_Button(object sender, EventArgs e)
    {
        string email = Email_Entry_SignIn.Text;
        string password = Password_Entry_SignIn.Text;
        if (email == null || password == null)
        {
            DisplayAlert("Ошибка", "Заполните все поля", "OK");
            return;
        }
        else
        {
            if (_signInViewModel.SignInUser(email, password))
            {
               // DisplayAlert("Успех", "Вход выполнен успешно", "OK");
               if (email == "admin" && password == "admin")
                {
                     Navigation.PushModalAsync(new AdminMainPage());
                }
               else
                Navigation.PushModalAsync(new UserMainPage());
            }
            else
            {
                DisplayAlert("Ошибка", "Неверный email или пароль", "OK");
            }
        }
    }
}