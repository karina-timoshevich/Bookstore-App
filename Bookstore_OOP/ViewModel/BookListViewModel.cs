using Bookstore_OOP.Model;
using Bookstore_OOP.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Bookstore_OOP.View;

namespace Bookstore_OOP.ViewModel
{
    public partial class BookListViewModel : ObservableObject
    {
        [ObservableProperty]
        private BookDisplay _selectedBook;
        [ObservableProperty]
        private ObservableCollection<BookDisplay> _books;
        //[ObservableProperty]
        //private ObservableCollection<Author> _authors;
        DatabaseService dbService = new DatabaseService();
        public BookListViewModel()
        {
            dbService.InitDB();
            Books = new ObservableCollection<BookDisplay>(dbService.GetBooks());
            //  Authors = new ObservableCollection<Author>(dbService.GetAuthors());
        }
        [RelayCommand]
        private async Task RemoveBookAsync()
        {
            if (SelectedBook != null)
            {
                dbService.DeleteBook(SelectedBook.Book.Id);
                Books.Remove(SelectedBook);

                SelectedBook = null;
            }
            else
            {
                await Shell.Current.DisplayAlert("No book selected", "Please select and try again.", "Ok");
            }
        }
        [RelayCommand]
        private async Task AddBookAsync()
        {
            await Shell.Current.GoToAsync(nameof(AddBookView));
        }

        [RelayCommand]
        private async Task EditBook()
        {

        }

    }
}