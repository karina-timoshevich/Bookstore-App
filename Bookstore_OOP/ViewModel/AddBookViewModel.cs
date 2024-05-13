//using Android.OS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookstore_OOP.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Bookstore_OOP.Services;
using System.Diagnostics;

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

                // Debug.WriteLine("Book", Book.AuthorID.ToString());
                await Shell.Current.DisplayAlert("There is an empty field", "Please fill it out and try again.", "Ok");
            }
            else if (IsBookExist())
            {
                Book = new();
                await Shell.Current.DisplayAlert("This book is already in use", "Please try another one.", "Ok");
            }
            else
            {
                //await DatabaseService<User>.AddColumnAsync(User);
                // Debug.WriteLine("Book", Book.AuthorID);
                // Debug.WriteLine("Book", Book.Title);

                _dbService.AddBook(Book);
                await Shell.Current.DisplayAlert("Book added successfully", "Press Ok and return to the book list", "Ok");

                BackCommand.Execute(null);
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