namespace Bookstore_OOP.View;
using Bookstore_OOP.ViewModel;


public partial class AddBookView : ContentPage
{
    private AddBookViewModel _addBookViewModel;
    public AddBookView(AddUserViewModel addBookViewModel)
    {
        InitializeComponent();
        //BindingContext = addBookViewModel;
    }
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        _addBookViewModel = new();
        BindingContext = _addBookViewModel;

        // _userListViewModel.SelectedUser = null;

        base.OnNavigatedTo(args);
    }
}