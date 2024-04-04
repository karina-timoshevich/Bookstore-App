using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookstore_OOP.Model;
//using Windows.System;

namespace Bookstore_OOP.Services
{
    public class CartService
    {
        private List<CartItem> _cartItems;
        private DatabaseService _databaseService;

        public CartService(int userId, DatabaseService databaseService)
        {
            _databaseService = databaseService;
            // _cartItems = _databaseService.LoadCartItems(userId);
        }


    }
}