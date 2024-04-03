using Npgsql;
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
