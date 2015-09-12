using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ShootingRange.BusinessObjects;
using ShootingRange.ServiceDesk.ViewModel;

namespace ShootingRange.ServiceDesk.View.DesignTimeData
{
    public class DesignDataProvider
    {
        public ReassignProgramNumberViewModel ReassignProgramNumberDialog
        {
            get {  return new ReassignProgramNumberViewModel();}
        }

        public ReassignSessionViewModel ReassignSessionDialog
        {
            get
            {
                return new ReassignSessionViewModel
                {
                    Shooters = new ObservableCollection<PersonShooterViewModel>(new List<PersonShooterViewModel>
                    {
                        new PersonShooterViewModel
                        {
                            FirstName = "Hans",
                            LastName = "Schwegler",
                            ShooterNumber = 1232458
                        }
                    })
                };
            }
        }

        public ResultsPageViewModel ResultsPage
        {
            get
            {
                return new ResultsPageViewModel
                {
                    AllPersons = new List<PersonShooterViewModel>
                    {
                        new PersonShooterViewModel
                        {
                            FirstName = "Hans",
                            LastName = "Schwegler",
                            ShooterNumber = 124434,
                            DateOfBirth = new DateTime(2015, 02, 03),
                        },
                        new PersonShooterViewModel
                        {
                            FirstName = "Hansueli",
                            LastName = "Schweggimann",
                            ShooterNumber = 4334223,
                            DateOfBirth = new DateTime(1982, 12, 25),
                        },
                    },
                    PersonFilterText = "Han Schweg",
                    Sessions = new ObservableCollection<SessionViewModel>(new List<SessionViewModel>
                    {
                        new SessionViewModel
                        {
                            ProgramName = "Gruppenstich",
                            LaneNumber = 12,
                            Total = 125,
                        },
                        new SessionViewModel
                        {
                            ProgramName = "Sie & Er",
                            LaneNumber = 5,
                            Total = 145.2m,
                        },
                        new SessionViewModel
                        {
                            ProgramName = "Nachwuchsstich",
                            LaneNumber = 3,
                            Total = 123.5m,
                        },
                    }),
                    SelectedPerson = new PersonShooterViewModel
                    {
                        FirstName = "Hans",
                        LastName = "Schwegler",
                        ShooterNumber = 124434,
                        DateOfBirth = new DateTime(2015, 02, 03),
                    },
                    SelectedSession = new SessionViewModel
                    {
                        ProgramName = "Gruppenstich",
                        LaneNumber = 12,
                        Total = 125,
                        Shots = new ObservableCollection<Shot>(new List<Shot>
                        {
                            new Shot
                            {
                                PrimaryScore = 12.0m,
                                Ordinal = 1
                            },
                            new Shot
                            {
                                PrimaryScore = 8.2m,
                                Ordinal = 2
                            },
                            new Shot
                            {
                                PrimaryScore = 10.1m,
                                Ordinal = 3
                            },
                        }),
                        SelectedShot = new Shot
                        {
                            PrimaryScore = 12.0m,
                            Ordinal = 3
                        }
                    }
                };
            }
        }

        public PersonsPageViewModel PersonsPage
        {
            get
            {
                return new PersonsPageViewModel
                {
                    AllPersons = new List<UiPerson>
                    {
                        new UiPerson
                        {
                            FirstName = "Hans",
                            LastName = "Schwegler",
                            DateOfBirth = new DateTime(1982, 12, 21)
                        },
                        new UiPerson
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
                    SelectedPerson = new UiPerson
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