namespace Bookstore_OOP.View;

public partial class SignUpPage : ContentPage
{
	public SignUpPage()
	{
		InitializeComponent();
	}

    private async void TapGestureRecognizer_Tapped_For_SignIn(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//SignIn");
    }

    //private void Button_SignUp(object sender, EventArgs e)
    //{
    //    DatabaseService dbService = new DatabaseService();
    //    dbService.CreateTable();

    //    Bookstore_OOP.ViewModel.SignUpViewModel signUpViewModel = new Bookstore_OOP.ViewModel.SignUpViewModel();
    //    signUpViewModel.TestFunction();
    //}

    private void Button_SignUp(object sender, EventArgs e)
    {
        DatabaseService dbService = new DatabaseService();
        dbService.CreateTable();

        // Получение данных из полей ввода
        string name = NameEntry.Text;
        string email = EmailEntry.Text;
        string phoneNumber = MobileNumberEntry.Text;
        string password = PasswordEntry.Text;

        // Вставка данных в таблицу
        dbService.InsertData(name, email);

        Bookstore_OOP.ViewModel.SignUpViewModel signUpViewModel = new Bookstore_OOP.ViewModel.SignUpViewModel();
        signUpViewModel.TestFunction();
    }
}