using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore_OOP.Model
{
    public class Order : INotifyPropertyChanged
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        private int _id;
        public int Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        private int _userId;
        public int UserId
        {
            get { return _userId; }
            set
            {
                if (_userId != value)
                {
                    _userId = value;
                    OnPropertyChanged(nameof(UserId));
                }
            }
        }

        private DateTime _orderDate;
        public DateTime OrderDate
        {
            get { return _orderDate; }
            set
            {
                if (_orderDate != value)
                {
                    _orderDate = value;
                    OnPropertyChanged(nameof(OrderDate));
                }
            }
        }

        private decimal _totalPrice;
        public decimal TotalPrice
        {
            get { return _totalPrice; }
            set
            {
                if (_totalPrice != value)
                {
                    _totalPrice = value;
                    OnPropertyChanged(nameof(TotalPrice));
                }
            }
        }

        private List<OrderItem> _items;
        public List<OrderItem> Items
        {
            get { return _items; }
            set
            {
                if (_items != value)
                {
                    _items = value;
                    OnPropertyChanged(nameof(Items));
                }
            }
        }

        private string _status;
        public string Status
        {
            get { return _status; }
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged(nameof(Status));
                }
            }
        }

        public Order()
        {
            Items = new List<OrderItem>();
        }

        public void AddItem(OrderItem item)
        {
            Items.Add(item);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}