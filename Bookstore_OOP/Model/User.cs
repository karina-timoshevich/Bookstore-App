using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace Bookstore_OOP.Model
{
     public class User
     {
        private int _id;
        private string _email;
        private string _password;
        private string _Name;
        private string _phoneNumber;
       // private string _address;

        // [PrimaryKey, AutoIncrement]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get => _id; set => _id = value; }
        public string Email { get => _email; set => _email = value; }
        public string Password { get => _password; set => _password = value; }
        public string Name { get => _Name; set => _Name = value; }
        public string PhoneNumber { get => _phoneNumber; set => _phoneNumber = value; }
       // public string Address { get => _address; set => _address = value; }
    }
}
