using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore_OOP.Model
{
    public class CartItem
    {
        public int UserId { get; set; }
        public Book Book { get; set; }
        public int Quantity { get; set; }
    }
}