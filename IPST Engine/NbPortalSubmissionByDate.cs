using System;

namespace IPST_Engine
{
    public class NbPortalSubmissionByDate
    {
        public NbPortalSubmissionByDate(DateTime date, int number)
        {
            Date = date;
            Nb = number;
        }
        public virtual DateTime Date { get; set; }
        public virtual int Nb { get; set; }
    }
}
