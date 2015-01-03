using System;
using System.Collections.Generic;
using IPST_Engine;
using IPST_Engine.Repository;
using NFluent;
using Xunit;

namespace Tests
{
    public class DateComputationTest
    {
        [Fact]
        public void MaxDate_Test()
        {
            var lstPortalSubmission = new List<PortalSubmission>
            {
                new PortalSubmission
                {
                    DateSubmission = new DateTime(2014,1,1)
                }, // Only submission date is set
                new PortalSubmission
                {
                    DateSubmission = new DateTime(2014, 1, 1),
                    DateAccept = new DateTime(2014, 5, 1)
                }, // Portal accepted
                new PortalSubmission
                {
                    DateSubmission = new DateTime(2014, 1, 1),
                    DateReject = new DateTime(2014, 5, 1)
                }, // Portal NewRejected
                new PortalSubmission
                {
                    DateSubmission = new DateTime(2014, 1, 1),
                    DateReject = new DateTime(2014, 5, 1),
                    DateAccept = new DateTime(2014, 9, 1)
                } // Portal rejected and validated
            };
            Check.That(DateComputation.GetMaxDate(null)).Equals(DateTime.MinValue);
            Check.That(lstPortalSubmission[1].GetMaxDate()).Equals(new DateTime(2014, 5, 1));
            Check.That(lstPortalSubmission[2].GetMaxDate()).Equals(new DateTime(2014, 5, 1));
            Check.That(lstPortalSubmission[3].GetMaxDate()).Equals(new DateTime(2014, 9, 1));
            Check.That(lstPortalSubmission[0].GetMaxDate()).Equals(new DateTime(2014, 1, 1));
        }

        [Fact]
        public void GetLastNewTest()
        {
            Check.That(DateComputation.GetLastNew(null)).Equals(0);

            var portalSubmission = new PortalSubmission {UpdateTime = DateTime.Now.AddDays(-5)};
            Check.That(portalSubmission.GetLastNew()).Equals(5);
        }

        [Fact]
        public void GetTimeElaspedTest()
        {
            Check.That(DateComputation.GetTimeElasped(null)).IsNull();
            var portalSubmission = new PortalSubmission
            {

            };
            Check.That(portalSubmission.GetTimeElasped()).IsNull();
            portalSubmission = new PortalSubmission
            {
                DateSubmission = new DateTime(2014, 01,01),
                SubmissionStatus = SubmissionStatus.Pending
            };
            Check.That(portalSubmission.GetTimeElasped()).IsNull();
            portalSubmission.DateAccept = new DateTime(2014, 05, 01);
            portalSubmission.SubmissionStatus = SubmissionStatus.Accepted;
            Check.That(portalSubmission.GetTimeElasped()).Equals(120);
            portalSubmission = new PortalSubmission
            {
                DateSubmission = new DateTime(2014, 01, 01),
                DateReject = new DateTime(2014, 05, 01),
                SubmissionStatus = SubmissionStatus.Rejected
            };
            Check.That(portalSubmission.GetTimeElasped()).Equals(120);
        }
    }
}
