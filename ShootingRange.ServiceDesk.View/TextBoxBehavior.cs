using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ShootingRange.ServiceDesk.View
{
  public class TextBoxBehavior
  {
    #region Attached Property EscapeClearsText

    public static readonly DependencyProperty EscapeClearsTextProperty
       = DependencyProperty.RegisterAttached("EscapeClearsText", typeof(bool), typeof(TextBoxBehavior),
            new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnEscapeClearsTextChanged)));

    private static void OnEscapeClearsTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      if ((bool)e.NewValue)
      {
        var textBox = d as TextBox;
        if (textBox != null)
        {
          textBox.KeyUp -= TextBoxKeyUp;
          textBox.KeyUp += TextBoxKeyUp;
        }
      }
    }

    private static void TextBoxKeyUp(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Escape)
      {
        ((TextBox)sender).Text = string.Empty;
      }
    }

    public static void SetEscapeClearsText(DependencyObject dependencyObject, bool escapeClearsText)
    {
      if (!ReferenceEquals(null, dependencyObject))
        dependencyObject.SetValue(EscapeClearsTextProperty, escapeClearsText);
    }

    public static bool GetEscapeClearsText(DependencyObject dependencyObject)
    {
      if (!ReferenceEquals(null, dependencyObject))
        return (bool)dependencyObject.GetValue(EscapeClearsTextProperty);
      return false;
    }

    #endregion Attached Property EscapeClearsText
  }
}