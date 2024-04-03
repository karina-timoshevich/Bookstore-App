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
using System.Collections.ObjectModel;

namespace Bookstore_OOP.ViewModel
{
    public partial class UserListViewModel : ObservableObject
    {
        [ObservableProperty]
        private User _selectedUser;
        [ObservableProperty]
        private ObservableCollection<User> _users;
        DatabaseService dbService = new DatabaseService();
        public UserListViewModel()
        {
            dbService.InitDB();
            Users = new ObservableCollection<User>(dbService.GetUsers());
        }

    }
}
