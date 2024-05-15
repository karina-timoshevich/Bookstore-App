namespace Bookstore_OOP.View;
using Bookstore_OOP.ViewModel;
using Bookstore_OOP.Model;


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


    async void OnAuthorTextChanged(object sender, TextChangedEventArgs e)
    {
        await _addBookViewModel.FetchAuthors(e.NewTextValue);
        authorListView.IsVisible = !string.IsNullOrEmpty(e.NewTextValue);
    }


    void OnAuthorSelected(object sender, SelectedItemChangedEventArgs e)
    {
        var author = e.SelectedItem as Author;
        if (author == null)
        {
            // Если выбранный автор не существует в базе данных, добавляем его
            var newAuthor = new Author { FullName = authorEntry.Text };
            _addBookViewModel.AddAuthor(newAuthor);
            author = newAuthor;
        }
        authorEntry.Text = author.FullName;
        authorListView.IsVisible = false;
    }
}