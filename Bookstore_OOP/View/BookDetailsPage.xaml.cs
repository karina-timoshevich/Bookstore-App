using Bookstore_OOP.Model;
using Bookstore_OOP.ViewModel;
using Bookstore_OOP.Services;


namespace Bookstore_OOP.View
{
    public partial class BookDetailsPage : ContentPage
    {
        private BookDetailsViewModel viewModel;
        private DatabaseService _dbService = new DatabaseService();
        public BookDetailsPage(BookDisplay book)
        {
            InitializeComponent();
            _dbService.InitDB();
            viewModel = new BookDetailsViewModel(book);
            BindingContext = viewModel;
        }

        private void OnAddToCartClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var selectedBook = (BookDisplay)button.CommandParameter;
            if (selectedBook != null)
            {
                _dbService.AddBookToCart(_dbService.GetCurrentUser(), selectedBook.Book.Id);
            }
        }
    }

   
}