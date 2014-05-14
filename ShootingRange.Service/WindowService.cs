using System.Windows;
using ShootingRange.Service.Interface;
using ShootingRange.View;

namespace ShootingRange.Service
{
  public class WindowService : IWindowService
  {
    PersonCreateWindow _personCreateWindow;
    PersonEditWindow _personEditWindow;
    private ShooterCreateWindow _shooterCreateWindow;
    private ShooterEditWindow _shooterEditWindow;

    #region Person

    public void ShowCreatePersonWindow()
    {
      _personCreateWindow = new PersonCreateWindow();
      _personCreateWindow.ShowDialog();
    }

    public void CloseCreatePersonWindow()
    {
      CloseWindow(_personCreateWindow);
    }

    public void ShowEditPersonWindow()
    {
      _personEditWindow = new PersonEditWindow();
      _personEditWindow.ShowDialog();
    }

    public void CloseEditPersonWindow()
    {
      CloseWindow(_personEditWindow);
    }

    #endregion

    #region Shooter

    public void ShowCreateShooterWindow()
    {
      _shooterCreateWindow = new ShooterCreateWindow();
      _shooterCreateWindow.ShowDialog();
    }

    public void CloseCreateShooterWindow()
    {
      CloseWindow(_shooterCreateWindow);
    }

    public void ShowEditShooterWindow()
    {
      _shooterEditWindow = new ShooterEditWindow();
      _shooterEditWindow.ShowDialog();
    }

    public void CloseEditShooterWindow()
    {
      CloseWindow(_shooterEditWindow);
    }

    #endregion

    #region Dialogues

    public void ShowErrorMessage(string caption, string message)
    {
      MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Error);
    }

    public void ShowMessage(string caption, string message)
    {
      MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Information);
    }

    public bool ShowYesNoMessasge(string caption, string message)
    {
      throw new System.NotImplementedException();
    }

    public void ShowInscribeCompetitionDialog()
    {
      throw new System.NotImplementedException();
    }

    #endregion

    private void CloseWindow(Window window)
    {
      if (window != null)
      {
        window.Close();
      }
    }
  }
}