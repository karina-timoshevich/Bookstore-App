namespace Bookstore_OOP.View;
using Bookstore_OOP.ViewModel;
using Bookstore_OOP.Model;
using Bookstore_OOP.Services;
using System.Diagnostics;

public partial class DirectoryView : ContentPage
{
    private DirectoryViewModel _directoryViewModel;
    private DatabaseService _dbService = new DatabaseService();
    public DirectoryView()
    {
        InitializeComponent();
        _dbService.InitDB();
        _directoryViewModel = new DirectoryViewModel();
        BindingContext = _directoryViewModel;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }

    //private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    //{
    //    var selectedBook = (BookDisplay)e.SelectedItem;
    //    if (selectedBook != null)
    //    {
    //        Navigation.PushAsync(new BookDetailsPage(selectedBook));
    //    }
    //}

    private void OnAddToCartClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var selectedBook = (BookDisplay)button.CommandParameter;
        if (selectedBook != null)
        {
            _dbService.AddBookToCart(_dbService.GetCurrentUser(), selectedBook.Book.Id);
        }
    }


    private async void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var collectionView = (CollectionView)sender;
        var selectedBook = (BookDisplay)collectionView.SelectedItem;
        if (selectedBook != null)
        {
            await Navigation.PushModalAsync(new BookDetailsPage(selectedBook));
        }
    }

    private void OnImageTapped(object sender, EventArgs e)
    {
        var image = (Image)sender;
        var selectedBook = (BookDisplay)image.BindingContext;
        if (selectedBook != null)
        {
            Navigation.PushAsync(new BookDetailsPage(selectedBook));
        }
    }
}