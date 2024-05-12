namespace Bookstore_OOP.View;
using Bookstore_OOP.Model;

public partial class BookDetailsPage : ContentPage
{
    private BookDisplay _book;

    public BookDetailsPage(BookDisplay book)
    {
        InitializeComponent();
        _book = book;

        // Здесь вы можете использовать _book для инициализации свойств страницы
    }
}