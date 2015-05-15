using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using TestGoogleAPI.Model;

namespace TestGoogleAPI.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;

        /// <summary>
        /// The <see cref="WelcomeTitle" /> property's name.
        /// </summary>
        public const string WelcomeTitlePropertyName = "WelcomeTitle";

        private string _welcomeTitle = string.Empty;

        /// <summary>
        /// Gets the WelcomeTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string WelcomeTitle
        {
            get
            {
                return _welcomeTitle;
            }

            set
            {
                if (_welcomeTitle == value)
                {
                    return;
                }

                _welcomeTitle = value;
                RaisePropertyChanged(WelcomeTitlePropertyName);
            }
        }
        /// <summary>
        /// The <see cref="UserName" /> property's name.
        /// </summary>
        public const string UserNamePropertyName = "UserName";

        private string _sUserName;

        /// <summary>
        /// Sets and gets the UserName property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string UserName
        {
            get
            {
                return _sUserName;
            }   

            set
            {
                if (_sUserName == value)
                {
                    return;
                }

                _sUserName = value;
                RaisePropertyChanged(UserNamePropertyName);
            }
        }
        /// <summary>
        /// The <see cref="Password" /> property's name.
        /// </summary>
        public const string PasswordPropertyName = "Password";

        private string _Password ;

        /// <summary>
        /// Sets and gets the Password property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Password
        {
            get
            {
                return _Password;
            }

            set
            {
                if (_Password == value)
                {
                    return;
                }

                _Password = value;
                RaisePropertyChanged(PasswordPropertyName);
            }
        }
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IDataService dataService)
        {
            _dataService = dataService;
            _dataService.GetData(
                (item, error) =>
                {
                    if (error != null)
                    {
                        // Report error here
                        return;
                    }

                    WelcomeTitle = item.Title;
                });

        }
        private RelayCommand _loginCommand;
        private UserCredential credential;

        /// <summary>
        /// Gets the LoginCommand.
        /// </summary>
        public RelayCommand LoginCommand
        {
            get
            {
                return _loginCommand ?? (_loginCommand = new RelayCommand(
                    ExecuteLoginCommand));
            }
        }

        private void ExecuteLoginCommand()
        {
            if (!LoginCommand.CanExecute(null))
            {
                return;
            }
            using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                var task = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    new[] {GmailService.Scope.GmailReadonly },
                    "user", CancellationToken.None, new FileDataStore("Books.ListMyLibrary"));
                task.Wait();
                credential = task.Result;
            }
            
        }
        private RelayCommand _connectGMailCommand;
        private GmailService _service;
        private RelayCommand getLabelsCommand;

        /// <summary>
        /// Gets the GetLabelCommand.
        /// </summary>
        public RelayCommand GetLabelCommand
        {
            get
            {
                return getLabelsCommand ?? (getLabelsCommand = new RelayCommand(
                    ExecuteGetLabelCommand,
                    CanExecuteGetLabelCommand));
            }
        }

        private void ExecuteGetLabelCommand()
        {
            if (!GetLabelCommand.CanExecute(null))
            {
                return;
            }
            var labelsResponse = _service.Users.Labels.List("me").Execute();

        }

        private bool CanExecuteGetLabelCommand()
        {
            return true;
        }

        /// <summary>
        /// Gets the ConnectGMailCommand.
        /// </summary>
        public RelayCommand ConnectGMailCommand
        {
            get
            {
                return _connectGMailCommand ?? (_connectGMailCommand = new RelayCommand(
                    ExecuteConnectGMailCommand));
            }
        }

        private void ExecuteConnectGMailCommand()
        {
            if (!ConnectGMailCommand.CanExecute(null))
            {
                return;
            }
            _service = new GmailService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "IPST Windows"
            });    

        }


        private bool CanExecuteConnectGMailCommand()
        {
            return credential != null;
        }

        private RelayCommand _getEmailCommand;

        /// <summary>
        /// Gets the GetEmailCommand.
        /// </summary>
        public RelayCommand GetEmailCommand
        {
            get
            {
                return _getEmailCommand ?? (_getEmailCommand = new RelayCommand(
                    ExecuteGetEmailCommand,
                    CanExecuteGetEmailCommand));
            }
        }

        private void ExecuteGetEmailCommand()
        {
            if (!GetEmailCommand.CanExecute(null))
            {
                return;
            }
            var list = _service.Users.Messages.List("me");
            list.Q = string.Format("from:henridevos2@gmail.com subject:\"Portal review complete:\"");
            
            var messagesResponse = list.Execute();
            //var xmlSerialiser = new XmlSerializer(typeof (Message));
            foreach (var message in messagesResponse.Messages)
            {
                var getRequest = _service.Users.Messages.Get("me", message.Id);
                getRequest.Format = UsersResource.MessagesResource.GetRequest.FormatEnum.Full;

                var mesResponse = getRequest.Execute();
                var htmlEncoded = mesResponse.Payload.Parts.FirstOrDefault(p => p.MimeType == "text/html").Body.Data;
                var text = Encoding.UTF8.GetString(FromBase64ForUrlString(htmlEncoded));
                //var decoded = HttpUtility.UrlDecode(htmlEncoded);
                //xmlSerialiser.Serialize(stream, mesResponse);
            }
            //IPSTEngine engine = new IPSTEngine(null, null);
            //engine.ConnectAsync().Wait();
            //engine.CheckSubmissions();
        }

        private bool CanExecuteGetEmailCommand()
        {
            return true;
        }
        public static byte[] FromBase64ForUrlString(string base64ForUrlInput)
        {
            int padChars = (base64ForUrlInput.Length % 4) == 0 ? 0 : (4 - (base64ForUrlInput.Length % 4));
            StringBuilder result = new StringBuilder(base64ForUrlInput, base64ForUrlInput.Length + padChars);
            result.Append(String.Empty.PadRight(padChars, '='));
            result.Replace('-', '+');
            result.Replace('_', '/');
            return Convert.FromBase64String(result.ToString());
        }
        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}