﻿namespace ShootingRange.Service.Interface
{
  public interface IWindowService
  {
    void ShowCreatePersonWindow();
    void CloseCreatePersonWindow();
    void ShowEditPersonWindow();
    void CloseEditPersonWindow();
    void ShowCreateShooterWindow();
    void CloseCreateShooterWindow();
    void ShowEditShooterWindow();
    void CloseEditShooterWindow();

    bool ShowYesNoMessasge(string caption, string message);
    void ShowErrorMessage(string caption, string message);
    void ShowMessage(string caption, string message);
    void ShowInscribeCompetitionDialog();
  }
}