namespace Bookstore_OOP.View;
using Bookstore_OOP.ViewModel;
public partial class BookListView : ContentPage
{
    private BookListViewModel _bookListViewModel;
    public BookListView()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        _bookListViewModel = new();
        BindingContext = _bookListViewModel;

        // _userListViewModel.SelectedUser = null;

        base.OnNavigatedTo(args);
    }
}