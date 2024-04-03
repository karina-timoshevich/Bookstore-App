namespace Bookstore_OOP.View;
using Bookstore_OOP.ViewModel;

public partial class UserListView : ContentPage
{
    private UserListViewModel _userListViewModel;
    public UserListView()
	{
		InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        _userListViewModel = new();
        BindingContext = _userListViewModel;

        // _userListViewModel.SelectedUser = null;

        base.OnNavigatedTo(args);
    }
}