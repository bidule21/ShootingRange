using System;
using System.Globalization;
using System.Windows.Data;

namespace ShootingRange.View.Converter
{
  public class PersonNameShooterNumberConverter : IMultiValueConverter
  {
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
      if (values.Length != 3)
        return "unable to format name.";
      string lastName = string.Format("{0}", values[0]);
      string firstName = string.Format("{0}", values[1]);
      string shooterNumber = string.Format("{0}", values[2]);
      string formatType = (string) parameter;

      switch (formatType)
      {
        case "FormatLastFirst":
          return string.Format("{0}, {1} ({2})", lastName, firstName, shooterNumber);
        default:
          return string.Format("{1} {0} ({2})", lastName, firstName, shooterNumber);
      }
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException("Back conversion is currently not supported.");
    }
  }
}
