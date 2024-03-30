using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using System;
using System.Diagnostics;

public class DatabaseService
{
    private readonly string _connectionString;

    public DatabaseService()
    {
        _connectionString = "Host=localhost ;Username=karina ;Password=password ;Database=bookstore";
    }

    public void CreateTable()
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();
          //  Console.WriteLine("Соединение с базой данных открыто.");
            Debug.WriteLine("Соединение с базой данных открыто.");

            using (var command = new NpgsqlCommand())
            {
                command.Connection = connection;

                // Создание таблицы
                command.CommandText = @"CREATE TABLE IF NOT EXISTS Users (
                                        ID SERIAL PRIMARY KEY,
                                        Name VARCHAR(100),
                                        Email VARCHAR(100)
                                        );";
                command.ExecuteNonQuery();
               // Console.WriteLine("Таблица 'Users' успешно создана или уже существует.");
                Debug.WriteLine("Таблица 'Users' успешно создана или уже существует.");
            }
        }
    }

    public void InsertData(string name, string email)
    {
        using (var connection = new NpgsqlConnection(_connectionString))
        {
            connection.Open();

            using (var cmd = new NpgsqlCommand())
            {
                cmd.Connection = connection;

                // SQL-запрос для вставки данных
                cmd.CommandText = "INSERT INTO Users (Name, Email) VALUES (@name, @email)";
                cmd.Parameters.AddWithValue("name", name);
                cmd.Parameters.AddWithValue("email", email);


                cmd.ExecuteNonQuery();
            }
        }
    }
}

namespace Bookstore_OOP.ViewModel
{
    internal class SignUpViewModel
    {
        public void TestFunction()
        {
            Console.WriteLine("SignUpViewModel работает корректно.");
        }
    }
}
