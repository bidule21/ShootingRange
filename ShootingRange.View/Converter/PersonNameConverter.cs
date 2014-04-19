using System;
using System.Globalization;
using System.Windows.Data;

namespace ShootingRange.View.Converter
{
  public class PersonNameConverter : IMultiValueConverter
  {
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
      if (values.Length != 2)
        return "unable to format name.";
      string lastName = (string) values[0];
      string firstName = (string) values[1];
      string formatType = (string) parameter;

      switch (formatType)
      {
        case "FormatLastFirst":
          return string.Format("{0}, {1}", lastName, firstName);
        default:
          return string.Format("{1} {0}", lastName, firstName);
      }
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException("Back conversion is currently not supported.");
    }
  }
}
