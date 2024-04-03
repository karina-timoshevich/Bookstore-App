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

namespace Bookstore_OOP.ViewModel
{
    public partial class AddUserViewModel : ObservableObject //: BaseViewModel
    {
        [ObservableProperty]
        private User _user = new();
        DatabaseService _dbService;

        public AddUserViewModel(DatabaseService dbService)
        {
        _dbService = dbService;
        _dbService.InitDB();
        }
        private static bool IsAnyNullOrEmpty(object user)
        {
            return user.GetType()
                .GetProperties()
                .Where(pt => pt.PropertyType == typeof(string))
                .Select(v => (string)v.GetValue(user))
                .Any(value => string.IsNullOrWhiteSpace(value));
        }

        private bool IsUserExist()  
        {
           return _dbService.CheckEmailExists(_user.Email);
        }

        [RelayCommand]
        private static async Task BackAsync()
        {
            await Shell.Current.Navigation.PopAsync();
        }

        [RelayCommand]
        private async Task AddAsync()
        {
            if (IsAnyNullOrEmpty(User))
            {
                User = new();
                await Shell.Current.DisplayAlert("There is an empty field", "Please fill it out and try again.", "Ok");
            }
            else if (IsUserExist())
            {
                User = new();
                await Shell.Current.DisplayAlert("This e-mail is already in use", "Please try another one.", "Ok");
            }
            else
            {
                //await DatabaseService<User>.AddColumnAsync(User);
                _dbService.InsertData(_user.Name, _user.Email, _user.PhoneNumber, _user.Password);
                await Shell.Current.DisplayAlert("User added successfully", "Press Ok and return to the user list", "Ok");

                BackCommand.Execute(null);
            }
        }
    }
}
