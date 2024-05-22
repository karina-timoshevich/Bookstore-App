using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Bookstore_OOP.Model
{
    public class Amount
    {


        //   [JsonProperty("value")]
        public string Value { get; set; }

        //  [JsonProperty("currency")]
        public string Currency { get; set; }

        //[JsonIgnore]
        //public int OrderId { get; set; }
        //[JsonIgnore]
        //public Order Order { get; set; }

        //[JsonIgnore]
        //public string PaymentId { get; set; }
        //[JsonIgnore]
        //public Payment Payment { get; set; }

    }

    public class Confirmation
    {

        //  [JsonProperty("type")]
        public string Type { get; set; }

        //  [JsonProperty("confirmation_url")]
        public string Confirmation_url { get; set; }

        //[JsonIgnore]
        //public string? PaymentId { get; set; }
        //[JsonIgnore]
        //public Payment? Payment { get; set; }
    }

    public class Redirection
    {

        //  [JsonProperty("type")]
        public string Type { get; set; }

        //  [JsonProperty("confirmation_url")]
        public string Return_url { get; set; }
        //[JsonIgnore]
        //public int OrderId { get; set; }
        //[JsonIgnore]
        //public Order Order { get; set; }


    }
    public class Order : INotifyPropertyChanged
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        private int _id;
        [JsonIgnore]
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
        [JsonIgnore]
        private int _userId;
        [JsonIgnore]
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
        [JsonIgnore]
        private DateTime _orderDate;
        [JsonIgnore]
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
        [JsonIgnore]
        private decimal _totalPrice;
        [JsonIgnore]
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
        [JsonIgnore]
        private List<OrderItem> _items;
        [JsonIgnore]
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
        [JsonIgnore]
        private string _status;
        [JsonIgnore]
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

        //  [JsonProperty("amount")]
        public Amount Amount { get; set; }

        //  [JsonProperty("capture")]
        public bool Capture { get; set; }

        //  [JsonProperty("confirmation")]
        public Redirection Confirmation { get; set; }


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