using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookstore_OOP.Services;

namespace Bookstore_OOP.ViewModel
{
    public class SignInViewModel
    {
        readonly DatabaseService _dbService;
        public SignInViewModel(DatabaseService dbService)
        {
            this._dbService = dbService;
            dbService?.InitDB();
            //this._signInViewModel = signInViewModel;
        }
        public bool SignInUser(string email, string password)
        {
            return _dbService.CheckPassword(email, password);
           
        }
    }
}
