﻿using Npgsql;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Bookstore_OOP.Model;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore_OOP.Services
{
    public class DatabaseService
    {
        private readonly string _connectionString;
        DatabaseService dbService;
    
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
                                        Email VARCHAR(100),
                                        MobileNumber VARCHAR(100),
                                        Password VARCHAR(100)
                                        );";

                    command.ExecuteNonQuery();
                    // Console.WriteLine("Таблица 'Users' успешно создана или уже существует.");
                    Debug.WriteLine("Таблица 'Users' успешно создана или уже существует.");
                    // Проверка на существование админа в таблице
                    command.CommandText = @"SELECT EXISTS (SELECT 1 FROM Users WHERE Email = 'admin');";
                    var adminExists = (bool)command.ExecuteScalar();
                    // Если админа нет, то добавляем
                    if (!adminExists)
                    {
                        command.CommandText = @"INSERT INTO Users (Name, Email, MobileNumber, Password) 
                                VALUES ('-', 'admin', '-', 'admin');";
                        command.ExecuteNonQuery();
                        Debug.WriteLine("Администратор по умолчанию успешно добавлен.");
                    }


                    // Создание таблицы BannedUsers
                    command.CommandText = @"CREATE TABLE IF NOT EXISTS BannedUsers (
                                    UserID INTEGER REFERENCES Users(ID)
                                    );";
                    command.ExecuteNonQuery();
                    Debug.WriteLine("Таблица 'BannedUsers' успешно создана или уже существует.");


                    // Создание таблицы Authors
                    command.CommandText = @"CREATE TABLE IF NOT EXISTS Authors (
                                ID SERIAL PRIMARY KEY,
                                FullName VARCHAR(100)
                                );";
                    command.ExecuteNonQuery();
                    Debug.WriteLine("Таблица 'Authors' успешно создана или уже существует.");

                    // Создание таблицы Books
                    command.CommandText = @"CREATE TABLE IF NOT EXISTS Books (
                                ID SERIAL PRIMARY KEY,
                                Title VARCHAR(100),
                                AuthorID INTEGER REFERENCES Authors(ID),
                                Publisher VARCHAR(100),
                                Year INTEGER,
                                Genre VARCHAR(100),
                                Price DECIMAL
                                );";
                    command.ExecuteNonQuery();
                    Debug.WriteLine("Таблица 'Books' успешно создана или уже существует.");

                    // Создание таблицы Orders
                    command.CommandText = @"CREATE TABLE IF NOT EXISTS Orders (
                                ID SERIAL PRIMARY KEY,
                                UserID INTEGER REFERENCES Users(ID),
                                OrderDate DATE,
                                TotalPrice DECIMAL
                                );";
                    command.ExecuteNonQuery();
                    Debug.WriteLine("Таблица 'Orders' успешно создана или уже существует.");

                    // Создание таблицы OrderItems
                    command.CommandText = @"CREATE TABLE IF NOT EXISTS OrderItems (
                                ID SERIAL PRIMARY KEY,
                                OrderID INTEGER REFERENCES Orders(ID),
                                Quantity INTEGER
                                );";
                    command.ExecuteNonQuery();
                    Debug.WriteLine("Таблица 'OrderItems' успешно создана или уже существует.");
                }
            }
        }

        public void InsertData(string name, string email, string mobilenumber, string password)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;

                    // SQL-запрос для вставки данных
                    cmd.CommandText = "INSERT INTO Users (Name, Email, MobileNumber, Password) VALUES (@name, @email, @mobilenumber, @password)";
                    cmd.Parameters.AddWithValue("name", name);
                    cmd.Parameters.AddWithValue("email", email);
                    cmd.Parameters.AddWithValue("mobilenumber", mobilenumber);
                    cmd.Parameters.AddWithValue("password", password);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void AddBook(Book book)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;

                    // SQL-запрос для добавления новой книги
                    cmd.CommandText = "INSERT INTO Books (Title, AuthorID, Publisher, Year, Genre, Price) VALUES (@title, @authorId, @publisher, @year, @genre, @price)";
                    cmd.Parameters.AddWithValue("title", book.Title);
                    cmd.Parameters.AddWithValue("authorId", book.AuthorID);
                    cmd.Parameters.AddWithValue("publisher", book.Publisher);
                    cmd.Parameters.AddWithValue("year", book.Year);
                    cmd.Parameters.AddWithValue("genre", book.Genre);
                    cmd.Parameters.AddWithValue("price", book.Price);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public bool CheckEmailExists(string email)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;

                    // SQL-запрос для проверки существования email
                    cmd.CommandText = "SELECT EXISTS (SELECT 1 FROM Users WHERE Email = @email)";
                    cmd.Parameters.AddWithValue("email", email);

                    return (bool)cmd.ExecuteScalar();
                }
            }
        }

        public bool CheckBookExists(string title)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;

                    // SQL-запрос для проверки существования книги
                    cmd.CommandText = "SELECT EXISTS (SELECT 1 FROM Books WHERE Title = @title)";
                    cmd.Parameters.AddWithValue("title", title);

                    return (bool)cmd.ExecuteScalar();
                }
            }
        }

        public bool CheckPassword(string email, string password)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;

                    // SQL-запрос для получения пароля по email
                    cmd.CommandText = "SELECT Password FROM Users WHERE Email = @email";
                    cmd.Parameters.AddWithValue("email", email);

                    var result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        // Сравнение паролей
                        return result.ToString() == password;
                    }
                    else
                    {
                        // Пользователь с таким email не найден
                        return false;
                    }
                }
            }
        }

        public void InitDB()
        {
            dbService = new DatabaseService();
            dbService.CreateTable();
        }

        public List<User> GetUsers()
        {
            var users = new List<User>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;

                    // SQL-запрос для получения всех пользователей
                    cmd.CommandText = "SELECT * FROM Users";

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var user = new User
                            {
                                Id = (int)reader["ID"],
                                Name = (string)reader["Name"],
                                Email = (string)reader["Email"],
                                PhoneNumber = (string)reader["MobileNumber"],
                                Password = (string)reader["Password"]
                            };

                            users.Add(user);
                        }
                    }
                }
            }

            return users;
        }

        public List<Book> GetBooks()
        {
            var books = new List<Book>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;

                    // SQL-запрос для получения всех книг
                    cmd.CommandText = "SELECT * FROM Books";

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var book = new Book
                            {
                                Id = int.Parse((string)reader["ID"]),
                                Title = (string)reader["Title"],
                                AuthorID = int.Parse((string)reader["AuthorID"]),
                                Publisher = (string)reader["Publisher"],
                                Year = int.Parse((string)reader["Year"]),
                                Genre = (string)reader["Genre"],
                                Price = int.Parse((string)reader["Price"])
                            };

                            books.Add(book);
                        }
                    }
                }
            }

            return books;
        }

        public List<Author> GetAuthors()
        {
            var authors = new List<Author>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;

                    // SQL-запрос для выбора всех авторов
                    cmd.CommandText = "SELECT * FROM Authors";

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var author = new Author
                            {
                                Id = reader.GetInt32(0),
                                FullName = reader.GetString(1),
                                // Добавьте остальные поля автора, если они есть
                            };

                            authors.Add(author);
                        }
                    }
                }
            }

            return authors;
        }

        public void DeleteUser(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;

                    // SQL-запрос для удаления пользователя по ID
                    cmd.CommandText = "DELETE FROM Users WHERE ID = @id";
                    cmd.Parameters.AddWithValue("id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteBook(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;

                    // SQL-запрос для удаления книги по ID
                    cmd.CommandText = "DELETE FROM Books WHERE ID = @id";
                    cmd.Parameters.AddWithValue("id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public async Task BanUser(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;

                    // SQL-запрос для добавления пользователя в таблицу BannedUsers
                    cmd.CommandText = "INSERT INTO BannedUsers (UserID) VALUES (@id)";
                    cmd.Parameters.AddWithValue("id", id);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task UnbanUser(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;

                    // SQL-запрос для удаления пользователя из таблицы BannedUsers
                    cmd.CommandText = "DELETE FROM BannedUsers WHERE UserID = @id";
                    cmd.Parameters.AddWithValue("id", id);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public bool IsUserBannedById(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;

                    // SQL-запрос для проверки наличия пользователя в таблице BannedUsers по ID
                    cmd.CommandText = "SELECT EXISTS (SELECT 1 FROM BannedUsers WHERE UserID = @id)";
                    cmd.Parameters.AddWithValue("id", id);

                    return (bool)cmd.ExecuteScalar();
                }
            }
        }
    }
}
