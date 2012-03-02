using System;
using System.Windows.Data;
using System.Globalization;

namespace FolderLounge
{
    [ValueConversion(typeof(bool), typeof(string))]
    public class BooleanToPinnedTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            if ((bool)value)
                return "Bookmarks";
            else
                return "Recent";
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
