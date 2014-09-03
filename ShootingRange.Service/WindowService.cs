using System.Windows;
using ShootingRange.ConfigurationProvider;
using ShootingRange.Service.Interface;
using ShootingRange.View;

namespace ShootingRange.Service
{
  public class WindowService : IWindowService
  {
    private PersonCreateWindow _personCreateWindow;
    private PersonEditWindow _personEditWindow;
    private ShooterCreateWindow _shooterCreateWindow;
    private ShooterEditWindow _shooterEditWindow;
    private ParticipationCreateWindow _participationCreateWindow;
    private EditPassWindow _editPassWindow;
    private TextBoxInputWindow _textBoxInputWindow;

    #region Person

    public void ShowCreatePersonWindow()
    {
      _personCreateWindow = new PersonCreateWindow();
      ShowWindow(_personCreateWindow);
    }

    public void ShowEditPersonWindow()
    {
      _personEditWindow = new PersonEditWindow();
      ShowWindow(_personEditWindow);
    }

    public void CloseCreatePersonWindow()
    {
      CloseWindow(_personCreateWindow);
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
      ShowWindow(_shooterCreateWindow);
    }

    public void CloseCreateShooterWindow()
    {
      CloseWindow(_shooterCreateWindow);
    }

    public void ShowEditShooterWindow()
    {
      _shooterEditWindow = new ShooterEditWindow();
      ShowWindow(_shooterEditWindow);
    }

    public void CloseEditShooterWindow()
    {
      CloseWindow(_shooterEditWindow);
    }

    public void ShowCreateParticipationWindow()
    {
      _participationCreateWindow = new ParticipationCreateWindow();
      ShowWindow(_participationCreateWindow);
    }

    public void CloseCreateParticipationWindow()
    {
      CloseWindow(_participationCreateWindow);
    }

    public void ShowEditPassWindow()
    {
      _editPassWindow = new EditPassWindow();
      ShowWindow(_editPassWindow);
    }

    public void CloseEditPassWindow()
    {
      CloseWindow(_editPassWindow);
    }

    public void ShowTextBoxInputDialog(string caption, string message)
    {
      _textBoxInputWindow = new TextBoxInputWindow();
      ShowWindow(_textBoxInputWindow);
    }

    public void CloseTextBoxInputDialog()
    {
      CloseWindow(_textBoxInputWindow);
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
      MessageBoxResult result = MessageBox.Show(Application.Current.MainWindow, message, caption, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
      return result == MessageBoxResult.Yes;
    }

    public void ShowInscribeCompetitionDialog()
    {
      throw new System.NotImplementedException();
    }

    #endregion

    private void ShowWindow(Window window)
    {
      window.Owner = Application.Current.MainWindow;
      window.ShowDialog();
    }

    private void CloseWindow(Window window)
    {
      if (window != null)
      {
        window.Close();
      }
    }
  }
}