using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace IPST_Engine
{
    public interface IIPSTEngine
    {
        Task ConnectAsync(Stream clientSecretStream);
        Task<List<PortalSubmission>> GetPortalEmails(string subject, DateTime? getAfter, IProgress<SubmissionProgress> progress);
        Task CheckSubmissions(IProgress<SubmissionProgress> progress);
        IList<PortalSubmission> Pending { get; }
        IList<PortalSubmission> Rejected { get; }
        IList<PortalSubmission> Accepted { get; }
        IList<PortalSubmission> All { get; } 

    }
}