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

    private async void Sign_In_Button(object sender, EventArgs e)
    {
        string email = Email_Entry_SignIn.Text;
        string password = Password_Entry_SignIn.Text;
        if (email == null || password == null)
        {
           await DisplayAlert("������", "��������� ��� ����", "OK");
            return;
        }
        else
        {
            if (_signInViewModel.SignInUser(email, password))
            {
                // DisplayAlert("�����", "���� �������� �������", "OK");
                if (email == "admin" && password == "admin")
                {
                    //await Shell.Current.GoToAsync("//AdminMainPage");
                    Application.Current.MainPage = new AdminShell();
                }
                else
                {
                    //await Shell.Current.GoToAsync("//UserMainPage");
                    Application.Current.MainPage = new UserShell();
                }
                //await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("������", "�������� email ��� ������", "OK");
            }
        }
    }
}