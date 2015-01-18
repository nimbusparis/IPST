using System;

namespace IPST_Engine
{
    public enum SubmissionStatus
    {
        Pending, Accepted, Rejected, Ignored, Appealed
    }

    public enum RejectionReason
    {
        None, NotMeetCriteria, Duplicate, TooClose
    }
    public class PortalSubmission
    {
        public virtual DateTime? DateSubmission { get; set; }

        public virtual string Title { get; set; }

        public virtual int Id { get; set; }

        public virtual DateTime UpdateTime { get; set; }

        public virtual DateTime? DateAccept { get; set; }

        public virtual DateTime? DateReject { get; set; }

        public virtual RejectionReason RejectionReason { get; set; }

        public virtual Uri PortalUrl { get; set; }

        public virtual Uri ImageUrl { get; set; }

        public virtual SubmissionStatus SubmissionStatus { get; set; }

        public virtual string Description { get; set; }

        public virtual string PostalAddress { get; set; }

        public virtual string SubmitterEmail { get; set; }

        public virtual string SubmitterPseudo { get; set; }

    }
}