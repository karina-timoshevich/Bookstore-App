using System;
using System.Globalization;
using Bookstore_OOP.Services;

namespace Bookstore_OOP.Converters
{
    public class UserIdToBannedStatusConverter : IValueConverter
    {
        private DatabaseService _dbService;

        public UserIdToBannedStatusConverter()
        {
            _dbService = new DatabaseService();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int userId)
            {
                return _dbService.IsUserBanned(userId) ? Colors.Red : Colors.White;
            }

            return Colors.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}