using System;
using System.Globalization;
using System.Windows.Data;

namespace TreeViewTest.Controls
{
    public class HitCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var hitcount = value as int?;
            if (hitcount is null || hitcount.Value < 0)
                return string.Empty;

            return new string($" ({hitcount.Value})");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultur)
        {
            if (value is not string str) return -1;

            str = str.Trim(' ', '(', ')');
            if (int.TryParse(str, out int result))
                return result;

            return -1;
        }
    }
}
