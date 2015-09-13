using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ShootingRange.BusinessObjects;
using ShootingRange.Ranking;
using ShootingRange.ServiceDesk.ViewModel;

namespace ShootingRange.ServiceDesk.View.DesignTimeData
{
    public class DesignDataProvider
    {
        private DesignDataFactory df;
        public DesignDataProvider()
        {
            df = new DesignDataFactory();
        }

        public RankViewModel RankingPage
        {
            get
            {
                RankViewModel vm = new RankViewModel
                {
                    Participations = new ObservableCollection<ParticipationViewModel>
                    {
                      new ParticipationViewModel
                      {
                          ProgramNumber = 101,
                          ProgramName = "Gruppenstich"
                      }  
                    },
                    RankItems = new ObservableCollection<RankItem>
                    {
                        new RankItem
                        {
                            Rank = 1,
                            Label = string.Format("{0} {1}", df.FirstName, df.LastName),
                            Score = df.Score
                        },
                        new RankItem
                        {
                            Rank = 2,
                            Label = string.Format("{0} {1}", df.FirstName, df.LastName),
                            Score = df.Score

                        },
                        new RankItem
                        {
                            Rank = 3,
                            Label = string.Format("{0} {1}", df.FirstName, df.LastName),
                            Score = df.Score

                        },
                        new RankItem
                        {
                            Rank = 4,
                            Label = string.Format("{0} {1}", df.FirstName, df.LastName)
                            ,
                            Score = df.Score

                        },
                    }
                };
                return vm;
            }
        }

        public SelectShooterViewModel SelectShooter { get { return new SelectShooterViewModel
        {
            Shooters = new ObservableCollection<PersonShooterViewModel>
            {
                df.CreatePersonShooter(),
                df.CreatePersonShooter(),
                df.CreatePersonShooter(),
                df.CreatePersonShooter(),
            }
        };} }

        public ReassignProgramNumberViewModel ReassignProgramNumberDialog
        {
            get {  return new ReassignProgramNumberViewModel();}
        }

        public GroupsPageViewModel GroupsPage
        {
            get
            {
                ShooterCollectionViewModel sc = new ShooterCollectionViewModel
                {
                    CollectionName = "Sommerportmonee",
                    Shooters = new ObservableCollection<UiCollectionShooter>
                    {
                        df.CreateUiCollectionShooter(),
                        df.CreateUiCollectionShooter(),
                        df.CreateUiCollectionShooter(),
                        df.CreateUiCollectionShooter(),
                    }
                };

                GroupingPageGroupingViewModel vm = new GroupingPageGroupingViewModel
                {

                    GroupingType = "Gruppenstich",
                    ShooterCollections = new ObservableCollection<ShooterCollectionViewModel>
                    {
                        sc
                    },
                    SelectedShooterCollection = sc
                };

                return new GroupsPageViewModel
                {
                    Groupings =
                        new ObservableCollection<GroupingPageGroupingViewModel>(
                            new List<GroupingPageGroupingViewModel> { vm }),
                    SelectedGrouping = vm
                };
            }
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
                List<UiPerson> allPersons = new List<UiPerson>
                {
                    df.GetUiPerson(),
                    df.GetUiPerson(),
                    df.GetUiPerson(),
                    df.GetUiPerson(),
                    df.GetUiPerson(),
                };
                return new PersonsPageViewModel
                {
                    AllPersons = allPersons,
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
                                }),
                                Sessions = new ObservableCollection<SessionViewModel>
                                {
                                    new SessionViewModel
                                    {
                                        ProgramName = "Gruppenstich [102]",
                                        ShooterIsParticipating = true,
                                        LaneNumber = 9,
                                        Total = 125,
                                    }
                                },
                                SelectedSession = new SessionViewModel
                                {
                                    Shots = new ObservableCollection<Shot>
                                    {
                                        df.CreateShot(),
                                        df.CreateShot(),
                                        df.CreateShot(),
                                        df.CreateShot(),
                                        df.CreateShot(),
                                    }
                                }
                        },
                        new ShooterViewModel
                        {
                            Shooter = new Shooter
                            {
                                ShooterNumber = 1453358
                            }
                        }
                    }),
                    SelectedPerson = allPersons.First(),
                    PersonFilterText = string.Empty,
                };
            }
        }
    }
}