using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight.Command;
using IPST_Engine;
using IPST_GUI.Message;

namespace IPST_GUI.ViewModel
{

    public partial class MainViewModel
    {
        #region ConnectCommand

        private RelayCommand _connectCommandCommand;

        public RelayCommand ConnectCommand
        {
            get
            {
                return _connectCommandCommand
                       ?? (_connectCommandCommand = new RelayCommand(
                           ExecuteConnectCommandCommand
                           ));
            }
        }

        private async void ExecuteConnectCommandCommand()
        {
            var clientSecretStream = GetType().Assembly.GetManifestResourceStream("IPST_GUI.Resources.client_secret.json");
            await _gmailEngine.ConnectAsync(clientSecretStream);
        }

        #endregion

        #region CheckEmailsCommand

        private RelayCommand _checkEmailsCommandCommand;


        public RelayCommand CheckEmailsCommand
        {
            get
            {
                return _checkEmailsCommandCommand
                       ?? (_checkEmailsCommandCommand = new RelayCommand(
                           ExecuteCheckEmailsCommandCommand,
                           CanExecuteCheckEmailsCommandCommand
                           ));
            }
        }

        private async void ExecuteCheckEmailsCommandCommand()
        {
            try
            {
                ParsingInProgress = true;
                await _gmailEngine.CheckSubmissions(new Progress<SubmissionProgress>(UpdateProgressBar));
                RaisePropertyChanged(() => PendingNumber);
                RaisePropertyChanged(() => AcceptedNumber);
                RaisePropertyChanged(() => RejectedNumber);
                RaisePropertyChanged(() => AllNumber);
                RaisePropertyChanged(() => Pending);
                RaisePropertyChanged(() => Accepted);
                RaisePropertyChanged(() => Rejected);
                RaisePropertyChanged(() => All);
            }
            finally
            {
                ParsingInProgress = false;
            }
        }

        private bool CanExecuteCheckEmailsCommandCommand()
        {
            return !ParsingInProgress;
        }

        #endregion

        #region ShowPortalsCommand

        private RelayCommand<IList<PortalSubmission>> _showPortalsCommand;



        public RelayCommand<IList<PortalSubmission>> ShowPortalsCommand
        {
            get
            {
                return _showPortalsCommand
                       ?? (_showPortalsCommand = new RelayCommand<IList<PortalSubmission>>(
                           l => ExecuteShowPortalsCommand(l)
                           ));
            }
        }

        private void ExecuteShowPortalsCommand(IList<PortalSubmission> portalSubmissions)
        {
            var navigateAcceptedPortal = new MessageNavigatePortals(portalSubmissions);
            MessengerInstance.Send(navigateAcceptedPortal);
        }

        #endregion

        #region ChartsCommand

        private RelayCommand _chartsCommand;



        public RelayCommand ChartsCommand
        {
            get
            {
                return _chartsCommand
                       ?? (_chartsCommand = new RelayCommand(
                           ExecuteChartsCommand
                           ));
            }
        }

        private void ExecuteChartsCommand()
        {
            var navigateCharts = new MessageNavigateCharts();
            MessengerInstance.Send(navigateCharts);
        }

        #endregion


    }
}