using Bookstore_OOP.Services;
namespace Bookstore_OOP.View;
using Bookstore_OOP.ViewModel;

public partial class SignUpPage : ContentPage
{
    readonly DatabaseService _dbService;
    readonly SignUpViewModel _signUpViewModel;
	public SignUpPage(DatabaseService dbService, SignUpViewModel signUpViewModel )
	{
        this._dbService = dbService;
        dbService?.InitDB();
        this._signUpViewModel = signUpViewModel;
		InitializeComponent();
	}

    private async void TapGestureRecognizer_Tapped_For_SignIn(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//SignIn");
    }

    private void Button_SignUp(object sender, EventArgs e)
    {
        // ��������� ������ �� ����� �����
        string email = EmailEntry.Text;
        string name = NameEntry.Text;
        string phoneNumber = MobileNumberEntry.Text;
        string password = PasswordEntry.Text;
        
        if (email == null || name == null || phoneNumber == null || password == null)
        {
            DisplayAlert("������", "��������� ��� ����", "OK");
            return;
        }
        else if(_dbService.CheckEmailExists(email))
        {
            DisplayAlert("������", "������������ � ����� email ��� ����������", "OK");
            return;
        }

        else
        {
            DisplayAlert("�����", "����������� ������ �������", "OK");
            _signUpViewModel.SignUpUser(_dbService, name, email, phoneNumber, password);

            SignInViewModel signInViewModel = new SignInViewModel(_dbService);
            Navigation.PushModalAsync(new SignInPage(signInViewModel));
        }

    }
}