using System;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using IPST_Engine;
using IPST_Engine.Repository;
using IPST_GUI.Message;
using IPST_GUI.Properties;

namespace IPST_GUI.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class PortalViewModel : ViewModelBase
    {
        private readonly PortalSubmission _portalSubmission;
        private readonly IPortalSubmissionRepository _portalSubmissionRepository;

        /// <summary>
        /// Initializes a new instance of the PortalViewModel class.
        /// </summary>
        public PortalViewModel(PortalSubmission portalSubmission, IPortalSubmissionRepository portalSubmissionRepository)
        {
            _portalSubmission = portalSubmission;
            _portalSubmissionRepository = portalSubmissionRepository;
        }

        public PortalViewModel()
        {
            if (IsInDesignModeStatic)
            {
                _portalSubmission = new PortalSubmission
                {
                    Title = "The Test Title",
                    PortalUrl = new Uri("https://www.ingress.com/intel?ll=57.701806,11.960100&amp;z=18"),
                    ImageUrl = new Uri("http://lh5.ggpht.com/X86qj-jQN3fZf05XtY7zOTZU7QHrxN947uXlZkB0CUtFi0phczUO1N6ip4Gwx_yAkiF_Cxd3V8rjmwvuanZp"),
                    DateSubmission = new DateTime(2014,01,01),
                    DateAccept = new DateTime(2014, 05,01),
                    SubmissionStatus = SubmissionStatus.Accepted,
                    Description = "The description",
                    PostalAddress = "21, Avenue de la Vallée Heureuse, Soréde, France"
                };
            }
        }

        public string Title
        {
            get
            {
                if (_portalSubmission != null) return _portalSubmission.Title;
                return null;
            }
        }

        public string Description
        {
            get
            {
                if (_portalSubmission != null) return _portalSubmission.Description;
                return null;
            }
        }
        public Uri PortalImageUri
        {
            get
            {
                if (_portalSubmission != null) return _portalSubmission.ImageUrl;
                return null;
            }
        }
        public DateTime? SubmissionDate
        {
            get
            {
                if (_portalSubmission != null) return _portalSubmission.DateSubmission;
                return null;
            }
        }
        public DateTime? AcceptedDate
        {
            get
            {
                if (_portalSubmission != null) return _portalSubmission.DateAccept;
                return null;
            }
        }
        public DateTime? RejectedDate
        {
            get
            {
                if (_portalSubmission != null) return _portalSubmission.DateReject;
                return null;
            }
        }
        public DateTime? UpdateTime
        {
            get
            {
                if (_portalSubmission != null) return _portalSubmission.UpdateTime;
                return null;
            }
        }
        public SubmissionStatus SubmissionStatus
        {
            get
            {
                if (_portalSubmission != null) return _portalSubmission.SubmissionStatus;
                return SubmissionStatus.Ignored;
            }
        }

        public Uri PortalUri
        {
            get 
            { 
                if (_portalSubmission != null) return _portalSubmission.PortalUrl;
                return null;
            }
        }
        #region Computed properties
        public string RejectionReason
        {
            get
            {
                if (_portalSubmission != null)
                {
                    switch (_portalSubmission.RejectionReason)
                    {
                        case IPST_Engine.RejectionReason.None:
                            return string.Empty;
                        case IPST_Engine.RejectionReason.NotMeetCriteria:
                            return Resources.IDS_REJECTION_REASON_NOT_MEET_CRITERIA;
                        case IPST_Engine.RejectionReason.Duplicate:
                            return Resources.IDS_REJECTION_REASON_DUPLICATE;
                        case IPST_Engine.RejectionReason.TooClose:
                            return Resources.IDS_REJECTION_REASON_TOO_CLOSE;
                    }
                }
                return string.Empty;
            }
        }

        public ImageSource PortalImage
        {
            get
            {
                if (PortalImageUri == null) return null;
                return new BitmapImage(PortalImageUri);
            }
        }




        public int LastNew
        {
            get { return _portalSubmission.GetLastNew(); }
        }

        public int? Waiting
        {
            get { return _portalSubmission.GetTimeElasped(); }
        }

        public Brush DefaultThumbnailColor
        {
            get
            {
                if (_portalSubmission != null)
                {
                    switch (_portalSubmission.SubmissionStatus)
                    {
                        case SubmissionStatus.Pending:
                            return new SolidColorBrush(Colors.Gray);
                        case SubmissionStatus.Accepted:
                            return new SolidColorBrush(Colors.Green);
                        case SubmissionStatus.Rejected:
                            return new SolidColorBrush(Colors.Red);
                        case SubmissionStatus.Appealed:
                            return new SolidColorBrush(Colors.Blue);
                    }
                }
                return null;
            }
        }
        public string DefaultThumbnailSymbol
        {
            get
            {
                switch (_portalSubmission.SubmissionStatus)
                {
                    case SubmissionStatus.Pending:
                        return "s";
                    case SubmissionStatus.Accepted:
                        return "a";
                    case SubmissionStatus.Rejected:
                        return "r";
                    case SubmissionStatus.Appealed:
                        return "U";
                }
                return null;
            }
        }

        #endregion

        #region OpenSubmissionCommand

        private RelayCommand<PortalViewModel> _openSubmissionCommand;



        public RelayCommand<PortalViewModel> OpenSubmissionCommand
        {
            get
            {
                return _openSubmissionCommand
                       ?? (_openSubmissionCommand = new RelayCommand<PortalViewModel>(
                           p => ExecuteOpenSubmissionCommand(p)
                           ));
            }
        }

        private void ExecuteOpenSubmissionCommand(PortalViewModel submission)
        {
            var messagePortalDetails = new MessageNavigatePortal(submission);
            MessengerInstance.Send(messagePortalDetails);
        }

        #endregion
        #region AppealSubmissionCommand

        private RelayCommand _AppealSubmissionCommand;

         

        public RelayCommand AppealSubmissionCommand
        {
            get
            {
                return _AppealSubmissionCommand
                       ?? (_AppealSubmissionCommand = new RelayCommand(
                           () => ExecuteAppealSubmissionCommand()
                           ));
            }
        }

        private void ExecuteAppealSubmissionCommand()
        {
            _portalSubmission.SubmissionStatus = SubmissionStatus.Appealed;
            _portalSubmissionRepository.Save(_portalSubmission);
        }

        #endregion
        #region IgnoreSubmissionCommand

        private RelayCommand<PortalViewModel> _ignoreSubmissionCommand;


        public RelayCommand<PortalViewModel> IgnoreSubmissionCommand
        {
            get
            {
                return _ignoreSubmissionCommand
                       ?? (_ignoreSubmissionCommand = new RelayCommand<PortalViewModel>(
                           ExecuteIgnoreSubmissionCommand
                           ));
            }
        }

        public string PostalAddress
        {
            get { if (_portalSubmission != null) return _portalSubmission.PostalAddress;
                return null;
            }
        }

        private void ExecuteIgnoreSubmissionCommand(PortalViewModel model)
        {
            model._portalSubmission.SubmissionStatus = SubmissionStatus.Ignored;
            _portalSubmissionRepository.Save(_portalSubmission);
        }

        #endregion
        #region ManualAcceptCommand

        private RelayCommand _ManualAcceptCommand;

         

        public RelayCommand ManualAcceptCommand
        {
            get
            {
                return _ManualAcceptCommand
                       ?? (_ManualAcceptCommand = new RelayCommand(
                           () => ExecuteManualAcceptCommand()
                           ));
            }
        }

        private void ExecuteManualAcceptCommand()
        {
            MessengerInstance.Send<MessageManualAcceptPortal>(new MessageManualAcceptPortal(_portalSubmission));
        }


        #endregion
        #region NavigateIntelCommand

        private RelayCommand<Uri> _navigateIntelCommand;



        public RelayCommand<Uri> NavigateIntelCommand
        {
            get
            {
                return _navigateIntelCommand
                       ?? (_navigateIntelCommand = new RelayCommand<Uri>(
                           ExecuteNavigateIntelCommandCommand
                           ));
            }
        }

        private void ExecuteNavigateIntelCommandCommand(Uri uri)
        {
            Process.Start(new ProcessStartInfo(uri.ToString()));
        }

        #endregion

    }
}