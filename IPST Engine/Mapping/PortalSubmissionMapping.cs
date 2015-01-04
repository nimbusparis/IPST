using FluentNHibernate.Mapping;

namespace IPST_Engine.Mapping
{
    public class PortalSubmissionMapping : ClassMap<PortalSubmission>
    {
        public PortalSubmissionMapping()
        {
            Id(m => m.Id).GeneratedBy.Identity();
            Map(m => m.UpdateTime);
            Map(m => m.DateSubmission);
            Map(m => m.Title);
            Map(m => m.DateAccept);
            Map(m => m.DateReject);
            Map(m => m.ImageUrl);
            Map(m => m.PortalUrl);
            Map(m => m.SubmissionStatus);
            Map(m => m.Description);
            Map(m => m.RejectionReason);
            Map(m => m.PostalAddress);
            Map(m => m.SubmitterEmail);
            Map(m => m.SubmitterPseudo);
        }

    }
}