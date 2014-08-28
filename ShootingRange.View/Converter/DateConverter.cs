using System;
using System.Globalization;
using System.Windows.Data;

namespace ShootingRange.View.Converter
{
  internal class DateConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value != null)
        return ((DateTime) value).ToShortDateString();
      return String.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      string strValue = value.ToString();
      DateTime resultDateTime;
      return DateTime.TryParse(strValue, out resultDateTime) ? resultDateTime : value;
    }
  }
}
