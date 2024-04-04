
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

    DatabaseService dbService = new DatabaseService();
    public DirectoryViewModel()
    {
        dbService.InitDB();
        Books = new ObservableCollection<BookDisplay>(dbService.GetBooks());
    }

    [RelayCommand]
    private async Task AddToCartAsync(BookDisplay book)
    {
        if (book != null)
        {
            //   await Task.Run(() => dbService.AddBookToCart(userId, book.Book.Id, quantity));
        }

    }
}