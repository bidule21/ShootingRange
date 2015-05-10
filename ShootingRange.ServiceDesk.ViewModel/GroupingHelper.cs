using System;
using System.Collections.ObjectModel;
using System.Linq;
using Gui.ViewModel;
using ShootingRange.BusinessObjects;
using ShootingRange.ConfigurationProvider;
using ShootingRange.Repository.RepositoryInterfaces;

namespace ShootingRange.ServiceDesk.ViewModel
{
  public static class GroupingHelper
  {
    public static void ShowSelectGrouping(IWindow parent, Shooter shooter)
    {
      SelectGroupingViewModel vm = new SelectGroupingViewModel
      {
        Title = "Gruppierung auswählen"
      };
      vm.Initialize();

      bool? dialogResult = DialogHelper.ShowDialog(parent, vm, vm.Title);
      if (dialogResult.HasValue && dialogResult.Value)
      {
        if (vm.SelectedGrouping != null)
        {
          ICollectionShooterDataStore collectionShooterDataStore = ConfigurationSource.Configuration.GetCollectionShooterDataStore();
          
          CollectionShooter cs = new CollectionShooter
          {
            ShooterCollectionId = vm.SelectedGrouping.GroupingId,
            ShooterId = shooter.ShooterId
          };
          collectionShooterDataStore.Create(cs);
        }
      }
    }

    public static void ShowCreateGrouping(IWindow parent)
    {
      IParticipationDataStore participationDataStore = ConfigurationSource.Configuration.GetParticipationDataStore();
      IShooterCollectionDataStore shooterCollectionDataStore = ConfigurationSource.Configuration.GetShooterCollectionDataStore();
      IShooterCollectionParticipationDataStore shooterCollectionParticipationDataStore = ConfigurationSource.Configuration.GetShooterCollectionParticipationDataStore();

      Participation[] participations =
        participationDataStore.GetAll()
          .Where(p => p.AllowCollectionParticipation)
          .OrderBy(p => p.ParticipationName)
          .ToArray();

      CreateGroupingViewModel vm = new CreateGroupingViewModel()
      {
        Title = "Neue Gruppierung",
        Participations = new ObservableCollection<Participation>(participations)
      };
      bool? dialogResult = DialogHelper.ShowDialog(parent, vm, vm.Title);

      if (dialogResult.HasValue && dialogResult == true)
      {
        if (String.IsNullOrWhiteSpace(vm.ShooterCollection.CollectionName) || vm.SelectedParticipation == null)
        {
          ErrorInfoGroupingEdit(parent);
          return;
        }

        try
        {
          shooterCollectionDataStore.Create(vm.ShooterCollection);
          shooterCollectionParticipationDataStore.Create(new ShooterCollectionParticipation
          {
            ParticipationId = vm.SelectedParticipation.ParticipationId,
            ShooterCollectionId = vm.ShooterCollection.ShooterCollectionId
          });

          DialogHelper.ShowMessageDialog(parent, "Speichern erfolgreich", "Die Gruppierung wurde erfolgreich gespeichert.");
        }
        catch (Exception e)
        {
          DialogHelper.ShowErrorDialog(parent,
            "Speichern nicht erfolgreich.",
            "Die Gruppe konnte nicht gespeichert werden.\r\n\r\n" + e);
        }
      }
    }

    private static void ErrorInfoGroupingEdit(IWindow parent)
    {
      ErrorDialogViewModel errorVm = new ErrorDialogViewModel()
      {
        Caption = "Vorgang nicht möglich",
        MessageText = "Gruppenname und Gruppierungstyp dürfen nicht leer sein."
      };

      ViewServiceLocator.ViewService.ExecuteAction(parent, errorVm);
    }
  }
}