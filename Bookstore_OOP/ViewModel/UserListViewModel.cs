/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;
using Bookstore_OOP.Services;
using System.Collections.ObjectModel;
using Bookstore_OOP.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;*/


using Bookstore_OOP.Model;
using Bookstore_OOP.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Bookstore_OOP.View;


namespace Bookstore_OOP.ViewModel
{
    public partial class UserListViewModel : ObservableObject
    {
        [ObservableProperty]
        private User _selectedUser;
        [ObservableProperty]
        private ObservableCollection<User> _users;

        [ObservableProperty]
        private string _searchText;

        DatabaseService dbService = new DatabaseService();
        public UserListViewModel()
        {
            dbService.InitDB();
            Users = new ObservableCollection<User>(dbService.GetUsers().OrderBy(user => user.Name));
        }
        [RelayCommand]
        private async Task RemoveUserAsync()
        {
            if (SelectedUser != null)
            {
                //  await DatabaseService<User>.RemoveColumnAsync(SelectedUser.Id);
                dbService.DeleteUser(SelectedUser.Id);
                Users.Remove(SelectedUser);

                SelectedUser = null;
            }
            else
            {
                await Shell.Current.DisplayAlert("No user selected", "Please select and try again.", "Ok");
            }
        }


        [RelayCommand]
        private async Task AddUserAsync()
        {
            await Shell.Current.GoToAsync(nameof(AddUserView));
        }

        [RelayCommand]
        private async Task BunUser()
        {
            if (dbService.IsUserBannedById(_selectedUser.Id))
            {
                await dbService.UnbanUser(_selectedUser.Id);
                await Shell.Current.DisplayAlert("Title", "Unbun", "ok");
            }
            else
            {
                await dbService.BanUser(_selectedUser.Id);
                await Shell.Current.DisplayAlert("Title", "bun", "ok");
            }
        }

        [RelayCommand]
        private void SearchUser()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                Users = new ObservableCollection<User>(dbService.GetUsers());
            }
            else
            {
                Users = new ObservableCollection<User>(dbService.GetUsers().Where(user => user.Name.Contains(SearchText)));
            }
        }

    }
}