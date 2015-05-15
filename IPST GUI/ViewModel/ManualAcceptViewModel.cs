using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using IPST_Engine;
using IPST_Engine.Repository;
using Microsoft.Practices.Unity;

namespace IPST_GUI.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ManualAcceptViewModel : ViewModelBase
    {
        private readonly PortalSubmission _portalSubmission;
        private readonly IPortalSubmissionRepository _repository;

        /// <summary>
        /// Initializes a new instance of the ManualAcceptViewModel class.
        /// </summary>
        [InjectionConstructor]
        public ManualAcceptViewModel(PortalSubmission portalSubmission, IPortalSubmissionRepository repository)
        {
            _portalSubmission = portalSubmission;
            _repository = repository;

        }

        public ManualAcceptViewModel()
        {
            if (IsInDesignModeStatic)
            {
                _portalSubmission = new PortalSubmission
                {
                    Title = "The Test Title",
                    PortalUrl = new Uri("https://www.ingress.com/intel?ll=57.701806,11.960100&amp;z=18"),
                    ImageUrl = new Uri("http://lh5.ggpht.com/X86qj-jQN3fZf05XtY7zOTZU7QHrxN947uXlZkB0CUtFi0phczUO1N6ip4Gwx_yAkiF_Cxd3V8rjmwvuanZp"),
                    DateSubmission = new DateTime(2014, 01, 01),
                    DateAccept = new DateTime(2014, 05, 01),
                    SubmissionStatus = SubmissionStatus.Accepted,
                    Description = "The description",
                    PostalAddress = "21, Avenue de la Vallée Heureuse, Soréde, France"
                };
            }
            
        }
        #region SaveCommand

        private RelayCommand _SaveCommand;
        public string PortalTitle {get { return _portalSubmission.Title; }}


        public RelayCommand SaveCommand
        {
            get
            {
                return _SaveCommand
                       ?? (_SaveCommand = new RelayCommand(
                           () => ExecuteSaveCommand(),
                           () => CanExecuteSaveCommand()
                           ));
            }
        }
        private void ExecuteSaveCommand()
        {
            _portalSubmission.SubmissionStatus = SubmissionStatus.Accepted;
            _repository.Save(_portalSubmission);
        }

        private bool CanExecuteSaveCommand()
        {
            // Replace this condition if needed
            return true;
        }

        #endregion



        public Uri PortalUrl
        {
            get { return _portalSubmission.PortalUrl; }
            set
            {
                if (_portalSubmission.PortalUrl != value)
                {
                    _portalSubmission.PortalUrl = value;
                    RaisePropertyChanged(() => PortalUrl);
                }
            }
        }


        public DateTime AcceptedDate
        {
            get { return _portalSubmission.DateAccept??DateTime.MinValue; }
            set
            {
                if (_portalSubmission.DateAccept != value)
                {
                    _portalSubmission.DateAccept = value;
                    RaisePropertyChanged(() => AcceptedDate);
                }
            }
        }

         

        public string PostalAddress
        {
            get { return _portalSubmission.PostalAddress; }
            set
            {
                if (_portalSubmission.PostalAddress != value)
                {
                    _portalSubmission.PostalAddress = value;
                    RaisePropertyChanged(() => PostalAddress);
                }
            }
        }

        public ImageSource PortalImage
        {
            get
            {
                if (_portalSubmission == null || _portalSubmission.ImageUrl == null) return null;
                return new BitmapImage(_portalSubmission.ImageUrl);
            }
        }





    }
}