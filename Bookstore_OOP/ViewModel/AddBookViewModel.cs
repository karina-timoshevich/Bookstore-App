//using Android.OS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using Bookstore_OOP.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Bookstore_OOP.Services;
using System.Diagnostics;
using System.Globalization;

namespace Bookstore_OOP.ViewModel
{
    public partial class AddBookViewModel : ObservableObject //: BaseViewModel
    {
        [ObservableProperty]
        private Book _book = new();
        DatabaseService _dbService = new DatabaseService();
        [ObservableProperty]
        private ObservableCollection<Author> _authors;
        // [ObservableProperty]
        private Author _selectedAuthor;

        public Author SelectedAuthor
        {
            get { return _selectedAuthor; }
            set
            {
                _selectedAuthor = value;
                Book.AuthorID = value.Id;
            }
        }

        public AddBookViewModel()
        {

            _dbService.InitDB();
            Authors = new ObservableCollection<Author>(_dbService.GetAuthors());
            // Debug.WriteLine("COUNT", Authors.Count);
        }

        private string _fullName;
        public string FullName
        {
            get { return _fullName; }
            set
            {
                _fullName = value;
                OnPropertyChanged(nameof(FullName));
            }
        }
        private bool _isAuthorListVisible;
        public bool IsAuthorListVisible
        {
            get { return _isAuthorListVisible; }
            set
            {
                _isAuthorListVisible = value;
                OnPropertyChanged(nameof(IsAuthorListVisible));
            }
        }

        private static bool IsAnyNullOrEmpty(object book)
        {
            /////Add verification for int and decimal!!

            return book.GetType()
        .GetProperties()
        .Any(prop =>
        {
            var type = prop.PropertyType;
            var value = prop.GetValue(book);

            if (type == typeof(string))
            {
                return string.IsNullOrWhiteSpace((string)value);
            }

            return false;
        });
        }

        private bool IsBookExist()
        {
            return _dbService.CheckBookExists(_book.Title);
        }

        [RelayCommand]
        private static async Task BackAsync()
        {
            await Shell.Current.Navigation.PopAsync();
        }

        [RelayCommand]
        private async Task AddAsync()
        {
            if (IsAnyNullOrEmpty(Book))
            {
                Book = new();
                await Shell.Current.DisplayAlert("There is an empty field", "Please fill it out and try again.", "Ok");
            }
            else if (IsBookExist())
            {
                Book = new();
                await Shell.Current.DisplayAlert("This book is already in use", "Please try another one.", "Ok");
            }
            else if (Book.Price == null)
            {
                await Shell.Current.DisplayAlert("Invalid price", "Price must be a number.", "Ok");
            }
            else
            {
                string priceString = Book.Price.ToString();
                decimal parsedPrice;
                if (!decimal.TryParse(priceString, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out parsedPrice) || parsedPrice.ToString(CultureInfo.InvariantCulture) != priceString)
                {
                    await Shell.Current.DisplayAlert("Invalid price", "Price must be a number.", "Ok");
                }
                else if (Book.Year < 100 || Book.Year > DateTime.Now.Year)
                {
                    await Shell.Current.DisplayAlert("Invalid year", "Year must be between 100 and the current year.", "Ok");
                }
                else
                {
                    // Проверяем, существует ли автор в базе данных
                    var author = _dbService.GetAuthorByName(FullName);
                    if (author == null)
                    {
                        // Если автора нет, добавляем нового автора в базу данных
                        var newAuthor = new Author { FullName = this.FullName };
                        _dbService.AddAuthor(newAuthor);
                        Book.AuthorID = newAuthor.Id;
                    }
                    else
                    {
                        Book.AuthorID = author.Id;
                    }

                    _dbService.AddBook(Book);
                    await Shell.Current.DisplayAlert("Book added successfully", "Press Ok and return to the book list", "Ok");

                    BackCommand.Execute(null);
                }
            }
        }
        public async Task AddAuthor(Author author)
        {
            // Проверяем, существует ли автор в базе данных
            var existingAuthor = _dbService.GetAuthorByName(author.FullName);
            if (existingAuthor == null)
            {
                // Если автора нет, добавляем его в базу данных
                _dbService.AddAuthor(author);
            }
        }

        public async Task FetchAuthors(string input)
        {
            string _connectionString;
            _connectionString = "Host=192.168.1.103;Port=5432;Username=karina;Password=password;Database=bookstore";
            //_connectionString = "Host=10.0.2.2;Port=5432 ;Username=karina ;Password=password ;Database=bookstore";
            //_connectionString = "Host=localhost ;Username=karina ;Password=password ;Database=bookstore";
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Получаем авторов, которые соответствуют введенной строке
                using (var cmd = new NpgsqlCommand("SELECT * FROM Authors WHERE FullName LIKE @fullname", connection))
                {
                    cmd.Parameters.AddWithValue("fullname", $"%{input}%");

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        Authors.Clear();
                        while (await reader.ReadAsync())
                        {
                            Authors.Add(new Author
                            {
                                Id = reader.GetInt32(0),
                                FullName = reader.GetString(1)
                            });
                        }
                    }
                }
            }
        }

        //[RelayCommand]
        //private async Task SelectCoverAsync()
        //{
        //    var result = await FilePicker.PickAsync();
        //    if (result != null)
        //    {
        //        Book.CoverPath = result.FullPath;
        //    }
        //}


    }
}