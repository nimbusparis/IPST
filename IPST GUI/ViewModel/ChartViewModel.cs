using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using IPST_Engine;
using IPST_Engine.Repository;
using NHibernate.Linq;
using Remotion.Linq.Collections;

namespace IPST_GUI.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ChartViewModel : ViewModelBase
    {
        private readonly IPortalSubmissionRepository _repository;

        /// <summary>
        /// Initializes a new instance of the ChartViewModel class.
        /// </summary>
        public ChartViewModel(IPortalSubmissionRepository repository)
        {
            _repository = repository;
            MinDate = new DateTime(2012, 11, 15);
            MaxDate = DateTime.Today;
            if (IsInDesignMode)
            {
                SubmissionByDate = new ObservableCollection<NbPortalSubmissionByDate>
                {
                    new NbPortalSubmissionByDate(new DateTime(2014, 01, 01), 3),
                    new NbPortalSubmissionByDate(new DateTime(2014, 02, 01), 4),
                    new NbPortalSubmissionByDate(new DateTime(2014, 02, 05), 2),
                    new NbPortalSubmissionByDate(new DateTime(2014, 02, 15), 2),
                    new NbPortalSubmissionByDate(new DateTime(2014, 03, 01), 1),
                    new NbPortalSubmissionByDate(new DateTime(2014, 03, 05), 12),
                    new NbPortalSubmissionByDate(new DateTime(2014, 03, 25), 2),
                    new NbPortalSubmissionByDate(new DateTime(2014, 04, 05), 4),
                };
            }
        }

        private ObservableCollection<NbPortalSubmissionByDate> _SubmissionByDate;
         

        public ObservableCollection<NbPortalSubmissionByDate> SubmissionByDate
        {
            get { return _SubmissionByDate; }
            set
            {
                if (_SubmissionByDate != value)
                {
                    _SubmissionByDate = value;
                    RaisePropertyChanged(() => SubmissionByDate);
                }
            }
        }

        private DateTime _MinDate;
         

        public DateTime MinDate
        {
            get { return _MinDate; }
            set
            {
                if (_MinDate != value)
                {
                    _MinDate = value;
                    RaisePropertyChanged(() => MinDate);
                }
            }
        }

        private DateTime _MaxDate;
         

        public DateTime MaxDate
        {
            get { return _MaxDate; }
            set
            {
                if (_MaxDate != value)
                {
                    _MaxDate = value;
                    RaisePropertyChanged(() => MaxDate);
                }
            }
        }






        #region SubmissionChartCommand


        private RelayCommand<string> _SubmissionChartCommand;



        public RelayCommand<string> SubmissionChartCommand
        {
            get
            {
                return _SubmissionChartCommand
                       ?? (_SubmissionChartCommand = new RelayCommand<string>(
                           s => ExecuteSubmissionChartCommand(s)
                           ));
            }
        }

        private void ExecuteSubmissionChartCommand(string submissionStatus)
        {
            SubmissionByDate
                = new ObservableCollection<NbPortalSubmissionByDate>();

            _repository.GetNbPortalSubmissionByDates(
                (SubmissionStatus) Enum.Parse(typeof (SubmissionStatus), submissionStatus))
                .ForEach(p => SubmissionByDate.Add(p));
        }

        #endregion


    }
}