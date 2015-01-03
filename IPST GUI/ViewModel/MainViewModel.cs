using System;
using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight;
using IPST_Engine.Repository;
using IPST_Engine;

namespace IPST_GUI.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public partial class MainViewModel : ViewModelBase
    {
        private readonly IIPSTEngine _gmailEngine;
        private readonly IPortalSubmissionRepository _repository;


        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IIPSTEngine gmailEngine, IPortalSubmissionRepository repository)
        {
            _gmailEngine = gmailEngine;
            _repository = repository;
        }

        private bool _ParsingInProgress;
         

        public bool ParsingInProgress
        {
            get { return _ParsingInProgress; }
            set
            {
                if (_ParsingInProgress != value)
                {
                    _ParsingInProgress = value;
                    RaisePropertyChanged(() => ParsingInProgress);
                }
            }
        }



        private int _CurrentProcessing;

        public int CurrentProcessing
        {
            get { return _CurrentProcessing; }
            set
            {
                if (_CurrentProcessing != value)
                {
                    _CurrentProcessing = value;
                    RaisePropertyChanged(() => CurrentProcessing);
                }
            }
        }

        private int _MaxProcessing;
        public int MaxProcessing
        {
            get { return _MaxProcessing; }
            set
            {
                if (_MaxProcessing != value)
                {
                    _MaxProcessing = value;
                    RaisePropertyChanged(() => MaxProcessing);
                }
            }
        }



        private void UpdateProgressBar(Tuple<int, int> obj)
        {
            CurrentProcessing = obj.Item1;
            MaxProcessing = obj.Item2;
        }



        public int PendingNumber
        {
            get { return _gmailEngine.Pending == null ? 0 : _gmailEngine.Pending.Count; }
        }
        public int AcceptedNumber
        {
            get { return _gmailEngine.Accepted == null ? 0 : _gmailEngine.Accepted.Count; }
        }
        public int RejectedNumber
        {
            get { return _gmailEngine.Rejected == null ? 0 : _gmailEngine.Rejected.Count; }
        }
        public int AllNumber
        {
            get { return _gmailEngine.All == null ? 0 : _gmailEngine.All.Count; }
        }

        public IList<PortalSubmission> Pending
        {
            get { return _gmailEngine.Pending; }
        }
        public IList<PortalSubmission> Accepted
        {
            get { return _gmailEngine.Accepted; }
        }
        public IList<PortalSubmission> Rejected
        {
            get { return _gmailEngine.Rejected; }
        }
        public IList<PortalSubmission> All
        {
            get { return _gmailEngine.All; }
        }

        public int PercentAccepted
        {
            get
            {
                if (AcceptedNumber + RejectedNumber == 0) return 0;
                return AcceptedNumber * 100/(AcceptedNumber + RejectedNumber); 
            }
        }
        public int PercentRejected
        {
            get
            {
                if (AcceptedNumber + RejectedNumber == 0) return 0;
                return RejectedNumber * 100 / (AcceptedNumber + RejectedNumber);
            }
        }

        public int MeanWaitingTime
        {
            get { return 100; }
        }
        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}