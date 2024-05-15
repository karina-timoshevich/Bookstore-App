
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
        Books = new ObservableCollection<BookDisplay>(dbService.GetBooks());
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

    [RelayCommand]
    private async Task SearchBook()
    {
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            Books = new ObservableCollection<BookDisplay>(await dbService.GetBooksAsync());
        }
        else
        {
            var books = await dbService.GetBooksAsync();
            if (books != null)
            {
                Books = new ObservableCollection<BookDisplay>(books.Where(book => book.Book.Title.Contains(SearchText)));
            }
        }
    }
}