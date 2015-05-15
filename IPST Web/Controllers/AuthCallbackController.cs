using Google.Apis.Auth.OAuth2.Mvc;

namespace IPST_Web.Controllers
{
    public class AuthCallbackController :
            Google.Apis.Auth.OAuth2.Mvc.Controllers.AuthCallbackController
    {
        protected override FlowMetadata FlowData
        {
            get { return new AppAuthFlowMetadata(); }
        }
    }
}