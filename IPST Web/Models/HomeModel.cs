using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IPST_Web.Models
{
    public class HomeModel
    {
        public int NumberPending { get; private set; }
        public int NumberRejected { get; private set; }
        public int NumberValidated { get; private set; }
    }
}