using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Bookstore_OOP.View;
using Bookstore_OOP.Model;
using Bookstore_OOP.Services;
//using Windows.System;
namespace Bookstore_OOP.ViewModel;

public partial class DirectoryViewModel : ObservableObject
{

    [ObservableProperty]
    private ObservableCollection<BookDisplay> _books;
    //add selected book
    [ObservableProperty]
    private BookDisplay _selectedBook;

    DatabaseService dbService = new DatabaseService();
    public DirectoryViewModel()
    {
        dbService.InitDB();
        InitializeAsync();
    }

    private async void InitializeAsync()
    {
        Books = new ObservableCollection<BookDisplay>(await dbService.GetBooksAsync());
    }

    [RelayCommand]
    public async Task AddToCartAsync()
    {
        if (_selectedBook != null)
        {
            await Task.Run(() => dbService.AddBookToCart(dbService.GetCurrentUser(), _selectedBook.Book.Id));
        }

    }

    [ObservableProperty]
    private string _searchText;

    [ObservableProperty]
    private ObservableCollection<BookDisplay> _searchResults;

    [RelayCommand]
    public async Task SearchBookAsync()
    {
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            SearchResults = new ObservableCollection<BookDisplay>(await dbService.GetBooksAsync());
        }
        else
        {
            var books = await dbService.GetBooksAsync();
            SearchResults = new ObservableCollection<BookDisplay>(books.Where(book => book.Book.Title.Contains(SearchText)));
        }
    }
}