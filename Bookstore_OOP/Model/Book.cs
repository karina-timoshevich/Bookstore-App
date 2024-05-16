using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore_OOP.Model
{
        public class Book
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Id { get; set; }
            public string Title { get; set; }
            public int AuthorID { get; set; }
            public string Publisher { get; set; }
            public int Year { get; set; }
            public string Genre { get; set; }
            public decimal Price { get; set; }
            public string CoverPath { get; set; }
        }
}
