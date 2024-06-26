﻿using Npgsql;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Bookstore_OOP.Model;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookstore_OOP.ViewModel;
using System.Net.Http.Headers;
using System.Text.Json;


namespace Bookstore_OOP.Services
{
    public class DatabaseService
    {
        private readonly string _connectionString;
        DatabaseService dbService;
    
        public DatabaseService()
        {
            //_connectionString = "Host=192.168.1.103;Port=5432;Username=karina;Password=password;Database=bookstore";
            //_connectionString = "Host=10.0.2.2;Port=5432 ;Username=karina ;Password=password ;Database=bookstore";
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
                                Price DECIMAL,
                                CoverPath VARCHAR(255)
                                );";
                    command.ExecuteNonQuery();
                    Debug.WriteLine("Таблица 'Books' успешно создана или уже существует.");
                    // Создание таблицы CartItems
                    command.CommandText = @"CREATE TABLE IF NOT EXISTS CartItems (
                                ID SERIAL PRIMARY KEY,
                                UserId INTEGER REFERENCES Users(ID),
                                BookId INTEGER REFERENCES Books(ID),
                                Quantity INTEGER,
                                UNIQUE (UserId, BookId)
                                );";
                    command.ExecuteNonQuery();
                    Debug.WriteLine("Таблица 'CartItems' успешно создана или уже существует.");

                    // Создание таблицы Orders
                    command.CommandText = @"CREATE TABLE IF NOT EXISTS Orders (
                                ID SERIAL PRIMARY KEY,
                                UserID INTEGER REFERENCES Users(ID),
                                OrderDate DATE,
                                TotalPrice DECIMAL,
                                Status VARCHAR(100)
                                );";
                    command.ExecuteNonQuery();
                    Debug.WriteLine("Таблица 'Orders' успешно создана или уже существует.");

                    // Создание таблицы OrderItems
                    command.CommandText = @"CREATE TABLE IF NOT EXISTS OrderItems (
                                ID SERIAL PRIMARY KEY,
                                OrderID INTEGER REFERENCES Orders(ID),
                                BookID INTEGER REFERENCES Books(ID),
                                Quantity INTEGER
                                );";
                    command.ExecuteNonQuery();
                    Debug.WriteLine("Таблица 'OrderItems' успешно создана или уже существует.");


                    // Создание таблицы CurrentUsers
                    command.CommandText = @"CREATE TABLE IF NOT EXISTS CurrentUsers (
                        UserID INTEGER REFERENCES Users(ID)
                        );";
                    command.ExecuteNonQuery();
                    Debug.WriteLine("Таблица 'CurrentUsers' успешно создана или уже существует.");

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
                    cmd.CommandText = "INSERT INTO Books (Title, AuthorID, Publisher, Year, Genre, Price, CoverPath) VALUES (@title, @authorId, @publisher, @year, @genre, @price, @coverPath)";
                    cmd.Parameters.AddWithValue("title", book.Title);
                    cmd.Parameters.AddWithValue("authorId", book.AuthorID);
                    cmd.Parameters.AddWithValue("publisher", book.Publisher);
                    cmd.Parameters.AddWithValue("year", book.Year);
                    cmd.Parameters.AddWithValue("genre", book.Genre);
                    cmd.Parameters.AddWithValue("price", book.Price);
                    cmd.Parameters.AddWithValue("coverPath", book.CoverPath);

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
                    cmd.CommandText = "SELECT * FROM Users WHERE Email != 'admin'";

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

        public string GetAuthorNameById(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;

                    // SQL query to get the author's name by ID
                    cmd.CommandText = "SELECT FullName FROM Authors WHERE ID = @id";
                    cmd.Parameters.AddWithValue("id", id);

                    var result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        // Return the author's name
                        return result.ToString();
                    }
                    else
                    {
                        // No author with such ID found
                        return null;
                    }
                }
            }
        }

        public List<BookDisplay> GetBooks()
        {
            var books = new List<BookDisplay>();

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
                                Id = (int)reader["ID"],
                                Title = (string)reader["Title"],
                                AuthorID = (int)reader["AuthorID"],
                                Publisher = (string)reader["Publisher"],
                                Year = (int)reader["Year"],
                                Genre = (string)reader["Genre"],
                                Price = (decimal)reader["Price"],
                                CoverPath = (string)reader["CoverPath"]

                            };

                            var authorName = GetAuthorNameById(book.AuthorID);

                            books.Add(new BookDisplay
                            {
                                Book = book,
                                AuthorName = authorName
                            });
                        }
                    }
                }
            }

            return books;
        }
        public async Task<List<BookDisplay>> GetBooksAsync()
        {
            var books = new List<BookDisplay>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;

                    cmd.CommandText = "SELECT * FROM Books";

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var book = new Book
                            {
                                Id = (int)reader["ID"],
                                Title = (string)reader["Title"],
                                AuthorID = (int)reader["AuthorID"],
                                Publisher = (string)reader["Publisher"],
                                Year = (int)reader["Year"],
                                Genre = (string)reader["Genre"],
                                Price = (decimal)reader["Price"],
                                CoverPath = (string)reader["CoverPath"]
                            };

                            var authorName = GetAuthorNameById(book.AuthorID);

                            books.Add(new BookDisplay
                            {
                                Book = book,
                                AuthorName = authorName
                            });
                        }
                    }
                }
            }

            return books;
        }
        public List<CartItemDisplay> GetCartItems(int userId)
        {
            var cartItems = new List<CartItemDisplay>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;

                    // SQL-запрос для получения всех элементов корзины для данного пользователя
                    cmd.CommandText = "SELECT * FROM CartItems WHERE UserId = @userId";
                    cmd.Parameters.AddWithValue("userId", userId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var bookId = (int)reader["BookId"];
                            var quantity = (int)reader["Quantity"];

                            var book = GetBookById(bookId);
                            var authorName = GetAuthorNameById(book.AuthorID);

                            cartItems.Add(new CartItemDisplay
                            {
                                Book = book,
                                AuthorName = authorName,
                                Quantity = quantity
                            });
                        }
                    }
                }
            }

            return cartItems;
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

                    // SQL-запрос для удаления связанных записей в таблице CartItems
                    cmd.CommandText = "DELETE FROM CartItems WHERE BookID = @id";
                    cmd.Parameters.AddWithValue("id", id);

                    cmd.ExecuteNonQuery();

                    // Очистка параметров для следующего запроса
                    cmd.Parameters.Clear();

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

        public void SaveCartItems(List<CartItem> cartItems)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                foreach (var item in cartItems)
                {
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = connection;

                        // SQL-запрос для сохранения элементов корзины
                        cmd.CommandText = "INSERT INTO CartItems (UserId, BookId, Quantity) VALUES (@userId, @bookId, @quantity) ON CONFLICT (UserId, BookId) DO UPDATE SET Quantity = @quantity";
                        cmd.Parameters.AddWithValue("userId", item.UserId);
                        cmd.Parameters.AddWithValue("bookId", item.Book.Id);
                        cmd.Parameters.AddWithValue("quantity", item.Quantity);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public List<CartItem> LoadCartItems(int userId)
        {
            var cartItems = new List<CartItem>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;

                    // SQL-запрос для загрузки элементов корзины
                    cmd.CommandText = "SELECT * FROM CartItems WHERE UserId = @userId";
                    cmd.Parameters.AddWithValue("userId", userId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var bookId = (int)reader["BookId"];
                            var quantity = (int)reader["Quantity"];

                            var book = GetBookById(bookId);

                            cartItems.Add(new CartItem { UserId = userId, Book = book, Quantity = quantity });
                        }
                    }
                }
            }

            return cartItems;
        }

        public Book GetBookById(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;

                    // SQL-запрос для получения книги по ID
                    cmd.CommandText = "SELECT * FROM Books WHERE ID = @id";
                    cmd.Parameters.AddWithValue("id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Book
                            {
                                Id = (int)reader["ID"],
                                Title = (string)reader["Title"],
                                AuthorID = (int)reader["AuthorID"],
                                Publisher = (string)reader["Publisher"],
                                Year = (int)reader["Year"],
                                Genre = (string)reader["Genre"],
                                Price = (decimal)reader["Price"],

                                CoverPath = (string)reader["CoverPath"] // Add this line
                            };
                        }
                    }
                }
            }

            return null;
        }

        public void SetCurrentUser(int userId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;

                    // Удаление всех записей из таблицы CurrentUsers
                    cmd.CommandText = "DELETE FROM CurrentUsers";

                    cmd.ExecuteNonQuery();

                    // Добавление новой записи с идентификатором текущего пользователя
                    cmd.CommandText = "INSERT INTO CurrentUsers (UserID) VALUES (@userId)";
                    cmd.Parameters.AddWithValue("userId", userId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public int GetCurrentUser()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;

                    // Получение идентификатора текущего пользователя
                    cmd.CommandText = "SELECT UserID FROM CurrentUsers";

                    var result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        return (int)result;
                    }
                    else
                    {
                        // Текущий пользователь не установлен
                        //return null;
                        return 1;
                    }
                }
            }
        }

        public int GetUserIdByEmail(string email)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;

                    // SQL-запрос для получения идентификатора пользователя по email
                    cmd.CommandText = "SELECT ID FROM Users WHERE Email = @email";
                    cmd.Parameters.AddWithValue("email", email);

                    var result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        return (int)result;
                    }
                    else
                    {
                        // Пользователь с таким email не найден
                        // return null;
                        return 1;
                    }
                }
            }
        }

        public string GetUserNameById(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;

                    // SQL-запрос для получения имени пользователя по ID
                    cmd.CommandText = "SELECT Name FROM Users WHERE ID = @id";
                    cmd.Parameters.AddWithValue("id", id);

                    var result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        // Возвращаем имя пользователя
                        return result.ToString();
                    }
                    else
                    {
                        // Пользователь с таким ID не найден
                        return null;
                    }
                }
            }
        }

        public User GetUserById(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;

                    // SQL-запрос для получения всех полей пользователя по ID
                    cmd.CommandText = "SELECT * FROM Users WHERE ID = @id";
                    cmd.Parameters.AddWithValue("id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Создаем и возвращаем объект User
                            return new User
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("ID")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                Password = reader.GetString(reader.GetOrdinal("Password")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                PhoneNumber = reader.GetString(reader.GetOrdinal("MobileNumber")),
                                //Address = reader.GetString(reader.GetOrdinal("Address")),
                            };
                        }
                        else
                        {
                            // Пользователь с таким ID не найден
                            return null;
                        }
                    }
                }
            }
        }

        public void AddBookToCart(int userId, int bookId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;

                    // SQL-запрос для добавления книги в корзину
                    cmd.CommandText = "INSERT INTO CartItems (UserId, BookId, Quantity) VALUES (@userId, @bookId, 1) ON CONFLICT (UserId, BookId) DO NOTHING";
                    cmd.Parameters.AddWithValue("userId", userId);
                    cmd.Parameters.AddWithValue("bookId", bookId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void RemoveItem(int bookId, int userId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;

                    // SQL-запрос для удаления книги из корзины
                    cmd.CommandText = "DELETE FROM CartItems WHERE BookId = @bookId AND UserId = @userId";
                    cmd.Parameters.AddWithValue("bookId", bookId);
                    cmd.Parameters.AddWithValue("userId", userId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void IncreaseQuantity(int bookId, int userId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;

                    // SQL-запрос для увеличения количества книги в корзине
                    cmd.CommandText = "UPDATE CartItems SET Quantity = Quantity + 1 WHERE BookId = @bookId AND UserId = @userId";
                    cmd.Parameters.AddWithValue("bookId", bookId);
                    cmd.Parameters.AddWithValue("userId", userId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DecreaseQuantity(int bookId, int userId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;

                    // SQL-запрос для уменьшения количества книги в корзине
                    cmd.CommandText = "UPDATE CartItems SET Quantity = Quantity - 1 WHERE BookId = @bookId AND UserId = @userId AND Quantity > 0";
                    cmd.Parameters.AddWithValue("bookId", bookId);
                    cmd.Parameters.AddWithValue("userId", userId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public decimal GetBookPrice(int bookId)
        {
            decimal bookPrice = 0;

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;

                    // SQL-запрос для получения цены книги
                    cmd.CommandText = "SELECT Price FROM Books WHERE Id = @bookId";
                    cmd.Parameters.AddWithValue("bookId", bookId);

                    bookPrice = (decimal)cmd.ExecuteScalar();
                }
            }

            return bookPrice;
        }

        public void DeleteCartItemsByUserId(int userId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;

                    // SQL-запрос для удаления всех элементов из CartItems для данного пользователя
                    cmd.CommandText = "DELETE FROM CartItems WHERE UserId = @userId";
                    cmd.Parameters.AddWithValue("userId", userId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteLastUserOrder(int userId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;

                    // SQL-запрос для получения ID последнего заказа пользователя
                    cmd.CommandText = "SELECT MAX(Id) FROM Orders WHERE UserId = @userId";
                    cmd.Parameters.AddWithValue("userId", userId);

                    var orderId = (int)cmd.ExecuteScalar();

                    // SQL-запрос для удаления всех элементов заказа, связанных с последним заказом пользователя
                    cmd.CommandText = "DELETE FROM OrderItems WHERE OrderId = @orderId";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("orderId", orderId);

                    cmd.ExecuteNonQuery();

                    // SQL-запрос для удаления последнего заказа пользователя
                    cmd.CommandText = "DELETE FROM Orders WHERE Id = @orderId";

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public decimal GetTotalPrice(int userId)
        {
            decimal totalPrice = 0;

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;

                    // SQL-запрос для получения всех элементов корзины пользователя
                    cmd.CommandText = "SELECT * FROM CartItems WHERE UserId = @userId";
                    cmd.Parameters.AddWithValue("userId", userId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int quantity = (int)reader["Quantity"];
                            int bookId = (int)reader["BookId"];

                            // Получение цены книги
                            decimal bookPrice = GetBookPrice(bookId); // Метод для получения цены книги

                            // Увеличение общей стоимости
                            totalPrice += quantity * bookPrice;
                        }
                    }
                }
            }

            return totalPrice;
        }



        // Объявление события
        public event Action OrderAdded;
        public void PlaceOrder(int userId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                // Создание нового заказа
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;

                    // SQL-запрос для создания нового заказа
                    cmd.CommandText = "INSERT INTO Orders (UserId, OrderDate, TotalPrice, Status) VALUES (@userId, @orderDate, @totalPrice, @status) RETURNING Id";
                    cmd.Parameters.AddWithValue("userId", userId);
                    cmd.Parameters.AddWithValue("orderDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("totalPrice", GetTotalPrice(userId)); // Метод для получения общей стоимости всех элементов в корзине
                    cmd.Parameters.AddWithValue("status", "В обработке");

                    int orderId = (int)cmd.ExecuteScalar();

                    // Создание новых элементов заказа для каждого элемента в корзине
                    foreach (var cartItem in GetCartItems(userId)) // Метод для получения всех элементов в корзине
                    {
                        cmd.CommandText = "INSERT INTO OrderItems (OrderId, BookId, Quantity) VALUES (@orderId, @bookId, @quantity)";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("orderId", orderId);
                        cmd.Parameters.AddWithValue("bookId", cartItem.Book.Id);
                        cmd.Parameters.AddWithValue("quantity", cartItem.Quantity);

                        cmd.ExecuteNonQuery();
                    }
                }
            }

            // Вызов события после добавления заказа
            OrderAdded?.Invoke();
        }

        public List<Order> GetOrdersByUserId(int userId)
        {
            List<Order> orders = new List<Order>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;

                    // SQL-запрос для получения всех заказов пользователя
                    cmd.CommandText = "SELECT * FROM Orders WHERE UserId = @userId";
                    cmd.Parameters.AddWithValue("userId", userId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Order order = new Order
                            {
                                Id = (int)reader["Id"],
                                UserId = (int)reader["UserId"],
                                OrderDate = (DateTime)reader["OrderDate"], // Добавлено извлечение даты заказа
                                Status = (string)reader["Status"]
                            };

                            orders.Add(order);
                        }
                    }
                }
            }

            return orders;
        }

        public List<Order> GetAllOrders()
        {
            var orders = new List<Order>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;

                    // SQL query to get all orders
                    cmd.CommandText = "SELECT * FROM Orders";

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var order = new Order
                            {
                                Id = (int)reader["ID"],
                                UserId = (int)reader["UserID"],
                                OrderDate = (DateTime)reader["OrderDate"],
                                TotalPrice = (decimal)reader["TotalPrice"],
                                Status = (string)reader["Status"]
                            };

                            orders.Add(order);
                        }
                    }
                }
            }

            return orders;
        }

        public void UpdateOrderStatus(int orderId, string newStatus)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;

                    // SQL query to update the status of an order
                    cmd.CommandText = "UPDATE Orders SET Status = @newStatus WHERE ID = @orderId";
                    cmd.Parameters.AddWithValue("newStatus", newStatus);
                    cmd.Parameters.AddWithValue("orderId", orderId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Author GetAuthorByName(string name)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;

                    // SQL-запрос для получения автора по имени
                    cmd.CommandText = "SELECT * FROM Authors WHERE FullName = @fullname";
                    cmd.Parameters.AddWithValue("fullname", name);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Author
                            {
                                Id = reader.GetInt32(0),
                                FullName = reader.GetString(1)
                            };
                        }
                    }
                }
            }

            return null;
        }

        public void AddAuthor(Author author)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;

                    // SQL-запрос для добавления нового автора
                    cmd.CommandText = "INSERT INTO Authors (FullName) VALUES (@fullname) RETURNING Id";
                    cmd.Parameters.AddWithValue("fullname", author.FullName);

                    author.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void UpdateOrderConfirmationUrl(int orderId, string confirmationUrl)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;

                    // SQL-запрос для обновления URL подтверждения для заказа
                    cmd.CommandText = "UPDATE Orders SET comfirmation_return_url = @confirmationUrl WHERE Id = @orderId";
                    cmd.Parameters.AddWithValue("confirmationUrl", confirmationUrl);
                    cmd.Parameters.AddWithValue("orderId", orderId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public async Task<string> MakePayment(int userId, decimal totalprice)
        {
            Order order = null;

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;

                    // SQL-запрос для получения последнего заказа пользователя
                    cmd.CommandText = @"SELECT * FROM Orders WHERE UserId = @userId AND Id = (SELECT MAX(Id) FROM Orders WHERE UserId = @userId)";
                    cmd.Parameters.AddWithValue("userId", userId);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            order = new Order
                            {
                                Id = (int)reader["ID"],
                                UserId = (int)reader["UserID"],
                                OrderDate = (DateTime)reader["OrderDate"],
                                TotalPrice = (decimal)reader["TotalPrice"],
                                Status = (string)reader["Status"]
                            };
                        }
                    }
                }
            }

            if (order == null)
            {
                Debug.WriteLine("No order found for the given user.");
                return "No order found for the given user.";
            }
            order.Capture = true;
            order.Confirmation = new Redirection
            {
                Type = "redirect",
                Return_url = "https://github.com/karina-timoshevich"
            };
            order.Amount = new Amount
            {
                Value = order.TotalPrice.ToString(),
                Currency = "RUB"
            };
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            string json = System.Text.Json.JsonSerializer.Serialize(order, options);
            Debug.WriteLine('\n');
            Debug.WriteLine(json);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");


            var client = new HttpClient();
            var contentString = await content.ReadAsStringAsync();
            Debug.WriteLine(contentString);
            var byteArray = Encoding.ASCII.GetBytes("390747:test_hY31ddxSx520A4ARG0FcCqu0fbeKhDdVgtkiauX26TM");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://api.yookassa.ru/v3/payments"),
                Headers =
                     {
                         { "Idempotence-Key", DateTime.Now.ToString() },
                     },
                Content = content
            };

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var content_with_payment = await response.Content.ReadAsStringAsync();
                var options_2 = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                Payment payment = JsonSerializer.Deserialize<Payment>(content_with_payment, options);
                Debug.WriteLine(payment.Confirmation.Confirmation_url);
                Debug.WriteLine(content_with_payment);
                order.Confirmation.Return_url = payment.Confirmation.Confirmation_url;

                // Обновляем URL подтверждения в базе данных
                UpdateOrderConfirmationUrl(order.Id, payment.Confirmation.Confirmation_url);

                return payment.Confirmation.Confirmation_url;
                
            }
            else
            {
                Debug.WriteLine(response.StatusCode.ToString());
                return response.StatusCode.ToString();
            }
           
        }

        public bool IsUserBanned(int userId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;

                    // SQL-запрос для проверки, есть ли пользователь в таблице BannedUsers
                    cmd.CommandText = "SELECT COUNT(*) FROM BannedUsers WHERE UserID = @id";
                    cmd.Parameters.AddWithValue("id", userId);

                    var result = (long)cmd.ExecuteScalar();

                    return result > 0;
                }
            }
        }

        public List<Book> GetBooksByOrderId(int orderId)
        {
            List<Book> books = new List<Book>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"SELECT Books.*
                         FROM Books
                         INNER JOIN OrderItems ON Books.ID = OrderItems.BookID
                         WHERE OrderItems.OrderID = @OrderId";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OrderId", orderId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Book book = new Book
                            {
                                Id = reader.GetInt32(0),
                                Title = reader.GetString(1),
                                // Добавьте остальные поля книги здесь
                                Price = reader.GetDecimal(6),
                            };

                            books.Add(book);
                        }
                    }
                }
            }

            return books;
        }

        public Order GetOrderDetails(int orderId)
        {
            Order order = null;

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"SELECT Orders.ID, Orders.OrderDate, Orders.TotalPrice, OrderItems.BookID, Users.Name, Users.MobileNumber
                 FROM Orders
                 INNER JOIN OrderItems ON Orders.ID = OrderItems.OrderID
                 INNER JOIN Users ON Orders.UserID = Users.ID
                 WHERE Orders.ID = @OrderId";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OrderId", orderId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (order == null)
                            {
                                order = new Order
                                {
                                    Id = reader.GetInt32(0),
                                    OrderDate = reader.GetDateTime(1),
                                    TotalPrice = reader.GetDecimal(2),
                                    UserId = reader.GetInt32(3),
                                    Username = reader.GetString(4),
                                    PhoneNumber = reader.GetString(5),
                                    Books = new List<Book>()
                                };
                            }

                            var bookId = reader.GetInt32(3);
                            var book = GetBookById(bookId);
                            order.Books.Add(book);
                        }
                    }
                }
            }

            return order;
        }
    }

}