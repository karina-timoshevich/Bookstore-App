using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore_OOP.Model
{
    public class Cart
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public int UserId { get; set; }
    }
}