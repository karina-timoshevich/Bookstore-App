using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Bookstore_OOP.Model;
using Bookstore_OOP.Services;
using System.Threading.Tasks;

namespace Bookstore_OOP.ViewModel
{
    public partial class BookDetailsViewModel : ObservableObject
    {
        [ObservableProperty]
        private BookDisplay _selectedBook;

        private DatabaseService dbService = new DatabaseService();

        public BookDetailsViewModel(BookDisplay book)
        {
            dbService.InitDB();
            SelectedBook = book;
        }

        [RelayCommand]
        public async Task AddToCartAsync()
        {
            if (SelectedBook != null)
            {
                await Task.Run(() => dbService.AddBookToCart(dbService.GetCurrentUser(), SelectedBook.Book.Id));
            }
        }
    }
}