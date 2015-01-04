using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Resources;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using IPST_Engine.Repository;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;

namespace IPST_Engine
{
    public class IPSTEngine : IIPSTEngine
    {
        private readonly IPortalSubmissionRepository _repository;
        private readonly IPortalSubmissionParser _parser;
        private readonly IProgress<int> _progress;
        private readonly UnityContainer _container;
        private UserCredential credential;
        private GmailService _gmailService;

        public IPSTEngine(IPortalSubmissionRepository repository, IPortalSubmissionParser parser)
        {
            _repository = repository;
            _parser = parser;
        }

        public async Task ConnectAsync(Stream clientSecretStream)
        {
            credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                clientSecretStream,
                new[] { GmailService.Scope.GmailReadonly },
                "user", CancellationToken.None, new FileDataStore("IPST.Windows"));
            _gmailService =
                new GmailService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "IPST Windows"
                });

        }

        public IList<PortalSubmission> NewPendings { get; private set; }

        public IList<PortalSubmission> NewAccepted { get; private set; }

        public IList<PortalSubmission> NewRejected { get; private set; }

        public IList<PortalSubmission> Pending
        {
            get { return _repository.GetAll(submission => submission.SubmissionStatus == SubmissionStatus.Pending).OrderByDescending(p=>p.DateSubmission).ToList(); }
        }

        public IList<PortalSubmission> Accepted
        {
            get { return _repository.GetAll(submission => submission.SubmissionStatus == SubmissionStatus.Accepted).OrderByDescending(p => p.DateAccept).ToList(); }
        }

        public IList<PortalSubmission> All
        {
            get { return _repository.GetAll(); }
        }

        public IList<PortalSubmission> Rejected
        {
            get { return _repository.GetAll(submission => submission.SubmissionStatus == SubmissionStatus.Rejected).OrderByDescending(p => p.DateReject).ToList(); }
        }


        public async Task<List<PortalSubmission>> GetPortalEmails(string subject, DateTime? getAfter, IProgress<SubmissionProgress> progress)
        {
            var listPortalsSubmission = new List<PortalSubmission>();
            var list = _gmailService.Users.Messages.List("me");
            var query = "from:ingress-support@google.com";
            query = string.Format("{0} subject:{1}", query, subject);
            if (getAfter.HasValue)
            {
                query = string.Format("{0} after:{1:yyyy/MM/dd}", query, getAfter.Value);
            }
            list.Q = query;
            ListMessagesResponse messageResponse = null;
            var nbNewMessages = await CountMessages(list);
            var indexMessage = 0;
            do
            {
                messageResponse = await list.ExecuteAsync();
                if (messageResponse != null && messageResponse.Messages != null)
                {
                    foreach (var message in messageResponse.Messages)
                    {   
                        var messageRequest = _gmailService.Users.Messages.Get("me", message.Id);
                        messageRequest.Format = UsersResource.MessagesResource.GetRequest.FormatEnum.Full;
                        var response = await messageRequest.ExecuteAsync();
                        progress.Report(new SubmissionProgress(indexMessage, ((int?) nbNewMessages)??0)); 
                        indexMessage ++;
                        listPortalsSubmission.Add(_parser.ParseMessage(response));
                    }
                }
                list.PageToken = messageResponse.NextPageToken;
            } while (messageResponse.NextPageToken != null);
            return listPortalsSubmission;
        }

        private async Task<int> CountMessages(UsersResource.MessagesResource.ListRequest list)
        {
            long numberNewMails = 0;
            ListMessagesResponse messageResponse = null;
            do
            {
                messageResponse = await list.ExecuteAsync();
                if (messageResponse == null) break;
                
                numberNewMails += messageResponse.Messages!=null?messageResponse.Messages.Count:0;
                list.PageToken = messageResponse.NextPageToken;
            } while (messageResponse.NextPageToken != null);
            return (int)numberNewMails;
        }


        public async Task CheckSubmissions(IProgress<SubmissionProgress> progress)
        {
            var lastCheck = _repository.GetLastSubmissionDateTime();
            List<PortalSubmission> lstMessages = await GetPortalEmails("\"Ingress Portal\"", lastCheck, progress);
             // Remove the submission already parsed of the day
            var newMessages = new List<PortalSubmission>();
            foreach (var portalSubmission in lstMessages)
            {
                if (portalSubmission != null && _repository.GetByImageUrl(portalSubmission.ImageUrl) == null)
                    newMessages.Add(portalSubmission);
            }
            #region NewPendings
            NewPendings = newMessages.Where(p => p != null && p.SubmissionStatus == SubmissionStatus.Pending).ToList();
            _repository.Save(NewPendings);
            #endregion

            var portalsModified = new List<PortalSubmission>();
            #region NewAccepted

            NewAccepted = lstMessages.Where(p => p != null && p.SubmissionStatus == SubmissionStatus.Accepted).ToList();
            NewAccepted.ForEach(p =>
            {
                var existing = _repository.GetByImageUrl(p.ImageUrl);
                if (existing != null)
                {
                    existing.SubmissionStatus = SubmissionStatus.Accepted;
                    existing.UpdateTime = DateTime.Now;
                    existing.PortalUrl = p.PortalUrl;
                    existing.DateAccept = p.DateAccept;
                    existing.PostalAddress = p.PostalAddress;
                    existing.Description = p.Description;
                    portalsModified.Add(existing);
                    
                }
                else
                {
                    portalsModified.Add(p);
                }
            });
            #endregion

            #region NewRejected

            NewRejected = lstMessages.Where(p => p != null && p.SubmissionStatus == SubmissionStatus.Rejected).ToList();
            NewRejected.ForEach(p =>
            {
                var existing = _repository.GetByImageUrl(p.ImageUrl);
                if (existing != null)
                {
                    existing.SubmissionStatus = SubmissionStatus.Rejected;
                    existing.UpdateTime = DateTime.Now;
                    existing.PortalUrl = p.PortalUrl;
                    existing.DateReject = p.DateReject;       
                    existing.RejectionReason = p.RejectionReason;
                    portalsModified.Add(existing);
                }
                else
                {
                    portalsModified.Add(p);
                }
            });
            #endregion

            _repository.Save(portalsModified);
        }
    }
}