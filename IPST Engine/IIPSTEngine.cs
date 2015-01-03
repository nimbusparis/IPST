using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IPST_Engine
{
    public interface IIPSTEngine
    {
        Task ConnectAsync();
        Task<List<PortalSubmission>> GetPortalEmails(string subject, DateTime? getAfter, IProgress<Tuple<int, int>> progress);
        Task CheckSubmissions(IProgress<Tuple<int, int>> progress);
        IList<PortalSubmission> Pending { get; }
        IList<PortalSubmission> Rejected { get; }
        IList<PortalSubmission> Accepted { get; }
        IList<PortalSubmission> All { get; } 

    }
}