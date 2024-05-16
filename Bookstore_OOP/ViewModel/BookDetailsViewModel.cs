using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Bookstore_OOP.Model;
using Bookstore_OOP.Services;
using System.Threading.Tasks;
using System.ComponentModel;

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
            AuthorName = GetAuthorNameById(book.Book.AuthorID);
        }

        [RelayCommand]
        public async Task AddToCartAsync()
        {
            if (SelectedBook != null)
            {
                await Task.Run(() => dbService.AddBookToCart(dbService.GetCurrentUser(), SelectedBook.Book.Id));
            }
        }

        private string _authorName;
        public string AuthorName
        {
            get { return _authorName; }
            set
            {
                _authorName = value;
                OnPropertyChanged(nameof(AuthorName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string GetAuthorNameById(int id)
        {
            // Your existing method to get author name by id
            return dbService.GetAuthorNameById(id); 
        }
    }
}