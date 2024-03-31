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
                // Вставка данных в таблицу
                dbService.InsertData(name, email, phoneNumber, password);
        }
    }
}
