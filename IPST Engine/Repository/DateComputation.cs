using System;

namespace IPST_Engine.Repository
{
    public static class DateComputation
    {
        public static DateTime GetMaxDate(this PortalSubmission portalSubmission)
        {
            if (portalSubmission != null)
            {
                if (portalSubmission.DateAccept.HasValue)
                    return portalSubmission.DateAccept.Value;
                if (portalSubmission.DateReject.HasValue)
                    return portalSubmission.DateReject.Value;
                return portalSubmission.DateSubmission??DateTime.MinValue;
            }
            return DateTime.MinValue;
        }

        public static int GetLastNew(this PortalSubmission portalSubmission)
        {
            if (portalSubmission == null) return 0;
            return (DateTime.Now - portalSubmission.UpdateTime).Days;
        }

        public static int? GetTimeElasped(this PortalSubmission portalSubmission)
        {
            if (portalSubmission == null) return null;
            if (!portalSubmission.DateSubmission.HasValue)
                return null;
            if (portalSubmission.SubmissionStatus == SubmissionStatus.Pending)
                return null;
            if (portalSubmission.SubmissionStatus == SubmissionStatus.Accepted)
                return (portalSubmission.DateAccept.Value - portalSubmission.DateSubmission.Value).Days;
            if (portalSubmission.SubmissionStatus == SubmissionStatus.Rejected)
                return (portalSubmission.DateReject.Value - portalSubmission.DateSubmission.Value).Days;
            return 0;
        }
    }
}