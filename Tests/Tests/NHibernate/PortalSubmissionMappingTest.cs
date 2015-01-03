using System;
using IPST_Engine;
using IPST_Engine.Mapping;
using NFluent;
using Xunit;

namespace Tests.Nhibernate
{
    public class PortalSubmissionMappingTest : BaseNHibernateTest<PortalSubmissionMapping>
    {

        [Fact]
        public void Mapping_test()
        {
            var expected = new PortalSubmission
            {
                DateSubmission = new DateTime(2014, 01, 01),
                DateAccept = new DateTime(2014, 03,03),
                DateReject = new DateTime(2014, 04,04),
                Title = "Portal1",
                Description = "TheDescription",
                PortalUrl = new Uri("http://www.ingress.com/portail1"),
                ImageUrl = new Uri("http://www.images.com/hsdjbdhbejuhf_èrnjbn"),
                RejectionReason = RejectionReason.TooClose,
                SubmissionStatus = SubmissionStatus.Accepted,
                PostalAddress = "21, Avenue de la vallée Heureuse, Sorède, France",
                SubmitterEmail = "mandrillon@gmail.com",
                SubmitterPseudo = "Nimbusparis"

            };
            using (var session = OpenSession())
            {
                session.SaveOrUpdate(expected);
                var actual = session.QueryOver<PortalSubmission>().Where(q => q.Title == "Portal1").SingleOrDefault();
                Check.That(actual).HasFieldsWithSameValues(expected);
                
            }
        }
    }
}
