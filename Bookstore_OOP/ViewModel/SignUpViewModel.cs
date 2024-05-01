using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using System;
using System.Diagnostics;
using Microsoft.Maui.Controls;
using Bookstore_OOP.Services;
using System.Text.RegularExpressions;
using System.Windows.Input;



namespace Bookstore_OOP.ViewModel
{
    public class SignUpViewModel
    {
        public void TestFunction()
        {
            Console.WriteLine("SignUpViewModel работает корректно.");
        }

        public void SignUpUser(DatabaseService dbService, string name, string email, string phoneNumber, string password)
        {
            //if (!Regex.IsMatch(email, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*") && email != "admin")
            //{
            //    await Shell.Current.DisplayAlert("Invalid e-mail", "Please try again.", "Ok");
            //}
            // Вставка данных в таблицу
            dbService.InsertData(name, email, phoneNumber, password);
        }
    }
}
