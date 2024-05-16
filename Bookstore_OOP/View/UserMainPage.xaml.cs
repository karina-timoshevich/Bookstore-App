namespace Bookstore_OOP.View;
using Bookstore_OOP.Model;
using Bookstore_OOP.Services;
using Xamarin.Essentials;

public partial class UserMainPage : ContentPage
{
    private DatabaseService _dbService;
    public User CurrentUser { get; set; }

    public UserMainPage()
    {
        InitializeComponent();
        _dbService = new DatabaseService();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        int? userId = _dbService.GetCurrentUser();
        if (userId.HasValue)
        {
            User currentUser = _dbService.GetUserById(userId.Value);
            if (currentUser != null)
            {
                CurrentUser = currentUser;
                this.BindingContext = this;
            }
        }
    }
}