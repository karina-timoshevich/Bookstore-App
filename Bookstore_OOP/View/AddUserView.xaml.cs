namespace Bookstore_OOP.View;
using Bookstore_OOP.ViewModel;

public partial class AddUserView : ContentPage
{
    public AddUserView(AddUserViewModel addUserViewModel)
    {
        InitializeComponent();
        BindingContext = addUserViewModel;
    }
}