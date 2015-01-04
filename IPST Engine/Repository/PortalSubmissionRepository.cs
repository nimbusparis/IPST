using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;

namespace IPST_Engine.Repository
{
    public interface IPortalSubmissionRepository : IRepository<PortalSubmission, int>
    {
        PortalSubmission GetByImageUrl(Uri imageUrl);
        DateTime? GetLastSubmissionDateTime();
        IList<NbPortalSubmissionByDate> GetNbPortalSubmissionByDates(SubmissionStatus status);
    }

    public class PortalSubmissionRepository : RepositoryBase<PortalSubmission, int>, IPortalSubmissionRepository
    {
        public PortalSubmissionRepository(ISession session)
        {
            _session = session;
        }

        public PortalSubmission GetByImageUrl(Uri imageUrl)
        {
            var sqlQuery = string.Format("select {{p.*}} from PortalSubmission as p where ImageUrl like '%{0}'", imageUrl.AbsolutePath);
            var byImageUrl = _session.CreateSQLQuery(sqlQuery);
            byImageUrl.AddEntity("p", typeof (PortalSubmission));
            var portalSubmissions = byImageUrl.List<PortalSubmission>();
            return portalSubmissions.FirstOrDefault();
        }

        public DateTime? GetLastSubmissionDateTime()
        {
            var lastSubmission = _session.QueryOver<PortalSubmission>()
                .OrderBy(p => p.UpdateTime).Desc
                .Take(1).SingleOrDefault();
            if (lastSubmission != null) return lastSubmission.UpdateTime;
            return null;
        }

        public IList<NbPortalSubmissionByDate> GetNbPortalSubmissionByDates(SubmissionStatus status)
        {
            var submissions = _session.QueryOver<PortalSubmission>()
                .Where(p => p.SubmissionStatus == status)
                .List();
            switch (status)
            {
                case SubmissionStatus.Pending:
                    return submissions.GroupBy(p => p.DateSubmission.Value.Date)
                        .Select(g =>
                            new NbPortalSubmissionByDate(g.Key, g.Count()))
                            .ToList();
                case SubmissionStatus.Accepted:
                    return submissions.GroupBy(p => p.DateAccept.Value.Date)
                        .Select(g =>
                            new NbPortalSubmissionByDate(g.Key, g.Count()))
                            .ToList();
                case SubmissionStatus.Rejected:
                    return submissions.GroupBy(p => p.DateReject.Value.Date)
                        .Select(g =>
                            new NbPortalSubmissionByDate(g.Key, g.Count()))
                            .ToList();
            }
            return null;
        }

        public PortalSubmission ChangePortalStatus(Uri imageUrl, SubmissionStatus submissionStatus, RejectionReason rejectionReason = RejectionReason.None)
        {
            var portal = _session.QueryOver<PortalSubmission>()
                .Where(submission => submission.ImageUrl == imageUrl)
                .SingleOrDefault();
            if (portal != null)
                portal.SubmissionStatus = submissionStatus;
            switch (submissionStatus)
            {
                case SubmissionStatus.Accepted:
                    portal.DateAccept = DateTime.Now;
                    break;
                case SubmissionStatus.Rejected:
                    portal.DateReject = DateTime.Now;
                    portal.RejectionReason = rejectionReason;
                    break;
            }
            return portal;
        }

        public IList<PortalSubmission> GetAllByStatus(SubmissionStatus submissionStatus)
        {
            return _session.QueryOver<PortalSubmission>().Where(q=>q.SubmissionStatus == submissionStatus).List<PortalSubmission>();
        }

        public static bool CompareUri(Uri uri1, Uri uri2)
        {
            if (uri1 == null && uri2 == null) return true;
            if (uri1 == null || uri2 == null) return false;
            return uri1.AbsolutePath == uri2.AbsolutePath;
        }
    }
}