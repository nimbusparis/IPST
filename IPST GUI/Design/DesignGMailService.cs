using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using IPST_Engine;

namespace IPST_GUI.Design
{
    public class DesignIPSTService : IIPSTEngine
    {
        public async Task ConnectAsync(Stream clientSecretStream)
        {
        }

        public IList<PortalSubmission> Pending {
            get
            {
                var result = new List<PortalSubmission>
                {
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Pending} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Pending} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Pending} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Pending} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Pending} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Pending} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Pending} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Pending} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Pending} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Pending} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Pending} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Pending} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Pending} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Pending} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Pending} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Pending} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Pending} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Pending} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Pending} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Pending} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Pending} ,
                };
                return result;
            } 
        }

        public IList<PortalSubmission> Accepted
        {
            get
            {
                var result = new List<PortalSubmission>
                {
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Accepted} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Accepted} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Accepted} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Accepted} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Accepted} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Accepted} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Accepted} ,
                };
                return result;
            }
        }

        public IList<PortalSubmission> Appealed
        {
            get
            {
                var result = new List<PortalSubmission>
                {
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Appealed} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Appealed} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Appealed} ,
                };
                return result;
            }
        }

        public IList<PortalSubmission> Rejected {
            get
            {
                var result = new List<PortalSubmission>
                {
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Rejected} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Rejected} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Rejected} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Rejected} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Rejected} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Rejected} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Rejected} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Rejected} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Rejected} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Rejected} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Rejected} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Rejected} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Rejected} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Rejected} ,
                };
                return result;
            }
        }

        public IList<PortalSubmission> All
        {
            get
            {
                var result = new List<PortalSubmission>
                {
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Pending} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Pending} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Pending} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Pending} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Pending} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Pending} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Pending} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Pending} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Pending} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Pending} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Pending} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Pending} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Pending} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Pending} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Pending} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Pending} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Pending} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Pending} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Pending} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Pending} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Pending} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Accepted} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Accepted} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Accepted} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Accepted} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Accepted} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Accepted} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Accepted} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Rejected} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Rejected} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Rejected} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Rejected} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Rejected} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Rejected} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Rejected} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Rejected} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Rejected} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Rejected} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Rejected} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Rejected} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Rejected} ,
                    new PortalSubmission {SubmissionStatus = SubmissionStatus.Rejected} ,
                };
                return result;
            }
        }

        public Task<List<PortalSubmission>> GetPortalEmails(string subject, DateTime? getAfter, IProgress<SubmissionProgress> progress)
        {
            return null;
        }

        public async Task CheckSubmissions(IProgress<SubmissionProgress> progress)
        {
            
        }

    }
}