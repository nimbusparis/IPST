using System;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using IPST_Engine;

namespace IPST_GUI.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class PortalsViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the PortalsViewModel class.
        /// </summary>
        public PortalsViewModel()
        {
            if (IsInDesignModeStatic)
            {
                _portalSubmissions = new ObservableCollection<PortalViewModel>
                {
                    new PortalViewModel(new PortalSubmission
                    {
                        Title = "Portal 1",
                        DateSubmission = new DateTime(2014, 01, 1),
                        SubmissionStatus = SubmissionStatus.Pending,
                        UpdateTime = new DateTime(2014, 05,05),
                    }, null),
                    new PortalViewModel(new PortalSubmission
                    {
                        Title = "Portal 2",
                        DateSubmission = new DateTime(2014, 01, 1),
                        DateAccept = new DateTime(2014, 05, 1),
                        SubmissionStatus = SubmissionStatus.Accepted,
                        UpdateTime = new DateTime(2014, 05,05),
                    }, null),
                    new PortalViewModel(new PortalSubmission
                    {
                        Title = "Portal 3",
                        DateSubmission = new DateTime(2014, 01, 1),
                        DateReject = new DateTime(2014, 06, 1),
                        SubmissionStatus = SubmissionStatus.Rejected,
                        UpdateTime = new DateTime(2014, 05,05),
                    }, null),
                    new PortalViewModel(new PortalSubmission
                    {
                        Title = "Portal 4",
                        DateSubmission = new DateTime(2014, 01, 1),
                        DateReject = new DateTime(2014, 07, 1),
                        SubmissionStatus = SubmissionStatus.Appealed,
                        UpdateTime = new DateTime(2014, 05,05),
                    }, null),
                };
            }
            DescChecked = true;
        }

        private ObservableCollection<PortalViewModel> _portalSubmissions;


        public ObservableCollection<PortalViewModel> PortalSubmissions
        {
            get { return _portalSubmissions; }
            set
            {
                if (_portalSubmissions != value)
                {
                    _portalSubmissions = value;
                    RaisePropertyChanged(() => PortalSubmissions);
                }
            }
        }

        private bool _submissionChecked;
         

        public bool SubmissionChecked
        {
            get { return _submissionChecked; }
            set
            {
                if (_submissionChecked != value)
                {
                    _submissionChecked = value;
                    RaisePropertyChanged(() => SubmissionChecked);
                }
            }
        }
        private bool _acceptedChecked;
         

        public bool AcceptedChecked
        {
            get { return _acceptedChecked; }
            set
            {
                if (_acceptedChecked != value)
                {
                    _acceptedChecked = value;
                    RaisePropertyChanged(() => AcceptedChecked);
                }
            }
        }
        private bool _rejectedChecked;


        public bool RejectedChecked
        {
            get { return _rejectedChecked; }
            set
            {
                if (_rejectedChecked != value)
                {
                    _rejectedChecked = value;
                    RaisePropertyChanged(() => RejectedChecked);
                }
            }
        }

        private bool _descChecked;


        public bool DescChecked
        {
            get { return _descChecked; }
            set
            {
                if (_descChecked != value)
                {
                    _descChecked = value;
                    RaisePropertyChanged(() => DescChecked);
                }
            }
        }




        #region OrderByCommand  

        private RelayCommand<string> _orderByCommand;



        public RelayCommand<string> OrderByCommand
        {
            get
            {
                return _orderByCommand
                       ?? (_orderByCommand = new RelayCommand<string>(
                           s => ExecuteOrderByCommand(),
                           s=>CanExecuteOrderByCommand(s)
                           ));
            }
        }

        private bool CanExecuteOrderByCommand(string s)
        {
            int count;
            switch (s)
            {
                case "Submission":
                    return true;
                case "Accepted":
                    count = PortalSubmissions.Where(
                        submission => submission.SubmissionStatus == SubmissionStatus.Accepted).Count();
                    return (count > 0);
                case "Rejected":
                    count = PortalSubmissions.Where(
                        submission => submission.SubmissionStatus == SubmissionStatus.Rejected).Count();
                    return (count > 0);
                case "Desc":
                    return true;
            }
            return false;
        }

        private void ExecuteOrderByCommand()
        {
            if (SubmissionChecked)
            {
                AcceptedChecked = RejectedChecked = false;
                PortalSubmissions = DescChecked ? new ObservableCollection<PortalViewModel>( PortalSubmissions.OrderBy(p => p.SubmissionDate).OrderByDescending(p=>p.SubmissionDate)) : new ObservableCollection<PortalViewModel>( PortalSubmissions.OrderBy(p => p.SubmissionDate));
            }
            if (AcceptedChecked)
            {
                SubmissionChecked = RejectedChecked = false;
                PortalSubmissions = DescChecked ? new ObservableCollection<PortalViewModel>(PortalSubmissions.OrderBy(p => p.AcceptedDate).OrderByDescending(p => p.AcceptedDate)) : new ObservableCollection<PortalViewModel>(PortalSubmissions.OrderBy(p => p.AcceptedDate));
            }
            if (RejectedChecked)
            {
                SubmissionChecked = AcceptedChecked = false;
                PortalSubmissions = DescChecked ? new ObservableCollection<PortalViewModel>(PortalSubmissions.OrderBy(p => p.RejectedDate).OrderByDescending(p=>p.RejectedDate)) : new ObservableCollection<PortalViewModel>(PortalSubmissions.OrderBy(p => p.RejectedDate));
            }
        }


        #endregion

        private RelayCommand _IgnoreSubmissionCommand;

         

        public RelayCommand IgnoreSubmissionCommand
        {
            get
            {
                return _IgnoreSubmissionCommand
                       ?? (_IgnoreSubmissionCommand = new RelayCommand(
                           () => ExecuteIgnoreSubmissionCommand(),
                           () => CanExecuteIgnoreSubmissionCommand()
                           ));
            }
        }

        private void ExecuteIgnoreSubmissionCommand()
        {
            // Add logic here
        }

        private bool CanExecuteIgnoreSubmissionCommand()
        {
            // Replace this condition if needed
            return true;
        }



    }
}