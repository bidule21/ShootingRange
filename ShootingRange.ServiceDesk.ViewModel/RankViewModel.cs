using System;
using System.Collections.ObjectModel;
using System.Linq;
using Gui.ViewModel;
using Microsoft.Practices.ServiceLocation;
using ShootingRange.ConfigurationProvider;
using ShootingRange.Ranking;

namespace ShootingRange.ServiceDesk.ViewModel
{
    public class RankViewModel : Gui.ViewModel.ViewModel
    {
        private ServiceDeskConfiguration _sdk;
        public ViewModelCommand GenerateRankingCommand { get; private set; }

        public RankViewModel()
        {
            GenerateRankingCommand = new ViewModelCommand(x => GenerateRanking(SelectedParticipation.ProgramNumber));
            GenerateRankingCommand.AddGuard(x => SelectedParticipation != null);
            GenerateRankingCommand.RaiseCanExecuteChanged();
        }

        public void Initialize()
        {
            _sdk = ServiceLocator.Current.GetInstance<ServiceDeskConfiguration>();
            Participations = new ObservableCollection<ParticipationViewModel>
            (
                _sdk.ParticipationDescriptions.GetAll().Select(x => new ParticipationViewModel
                {
                    ProgramNumber = int.Parse(x.ProgramNumber),
                    ProgramName = x.ProgramName
                })
            );
        }


        private ParticipationViewModel _selectedParticipation;

        public ParticipationViewModel SelectedParticipation
        {
            get { return _selectedParticipation; }
            set
            {
                if (value != _selectedParticipation)
                {
                    _selectedParticipation = value;
                    OnPropertyChanged();

                    GenerateRankingCommand.RaiseCanExecuteChanged();
                }
            }
        }


        private ObservableCollection<ParticipationViewModel> _participations;

        public ObservableCollection<ParticipationViewModel> Participations
        {
            get { return _participations; }
            set
            {
                if (value != _participations)
                {
                    _participations = value;
                    OnPropertyChanged();
                }
            }
        }

        private void GenerateRanking(int programNumber)
        {
            ParticipationDescription p = _sdk.ParticipationDescriptions.GetAll().SingleOrDefault(x => x.ProgramNumber == programNumber.ToString());

            if (p != null)
            {
                RankingType rankingType;
                bool success = Enum.TryParse(p.RankingType, out rankingType);

                if (success)
                {
                    RankingConfiguration config = new RankingConfigurationBuilder(programNumber, rankingType);
                    RankingReportBuilder reportBuilder = new RankingReportBuilder(config);

                    RankingReport report = reportBuilder.Build();
                    RankItems = new ObservableCollection<RankItem>(report.GetOrderedRankItems());
                }
            }
        }


        private ObservableCollection<RankItem> _rankItems;

        public ObservableCollection<RankItem> RankItems
        {
            get { return _rankItems; }
            set
            {
                if (value != _rankItems)
                {
                    _rankItems = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}