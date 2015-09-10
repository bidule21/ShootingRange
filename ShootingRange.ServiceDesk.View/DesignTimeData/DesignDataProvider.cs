using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ShootingRange.BusinessObjects;
using ShootingRange.ServiceDesk.ViewModel;

namespace ShootingRange.ServiceDesk.View.DesignTimeData
{
    public class DesignDataProvider
    {

        public PersonsPageViewModel PersonsPage
        {
            get
            {
                return new PersonsPageViewModel
                {
                    AllPersons = new List<Person>
                    {
                        new Person
                        {
                            FirstName = "Hans",
                            LastName = "Schwegler",
                            DateOfBirth = new DateTime(1982, 12, 21)
                        },
                        new Person
                        {
                            FirstName = "Hansueli",
                            LastName = "Schweggimann"
                        }
                    },
                    Shooters = new ObservableCollection<ShooterViewModel>(new List<ShooterViewModel>
                    {
                        new ShooterViewModel
                        {
                            Shooter = new Shooter
                            {
                                ShooterNumber = 123458
                            },
                            Participations =
                                new ObservableCollection<ParticipationViewModel>(new List<ParticipationViewModel>
                                {
                                    new ParticipationViewModel
                                    {
                                        ProgramNumber = 123,
                                        ProgramName = "Gruppenstich"
                                    },
                                    new ParticipationViewModel
                                    {
                                        ProgramNumber = 125,
                                        ProgramName = "Wortscht & Brot"
                                    }
                                }),
                                Groupings = new ObservableCollection<GroupingViewModel>(new List<GroupingViewModel>
                                {
                                    new GroupingViewModel
                                    {
                                        GroupingName = "Eichenlaub",
                                        ParticipationName = "Gruppentstich"
                                    }
                                })
                        },
                        new ShooterViewModel
                        {
                            Shooter = new Shooter
                            {
                                ShooterNumber = 1453358
                            }
                        }
                    }),
                    SelectedPerson = new Person
                    {
                        FirstName = "Hans",
                        LastName = "Schwegler"
                    },
                    PersonFilterText = "Han Schweg",
                };
            }
        }
    }
}