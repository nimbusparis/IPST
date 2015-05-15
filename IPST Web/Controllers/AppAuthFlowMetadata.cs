using System.Linq;
using System.Web.Mvc;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.Gmail.v1;
using Google.Apis.Util.Store;
using IPST_Web.App_Start;
using Microsoft.AspNet.Identity;

namespace IPST_Web.Controllers
{
    public class AppAuthFlowMetadata : FlowMetadata
    {
        private static IAuthorizationCodeFlow flow;

        static AppAuthFlowMetadata()
        {
            var name = typeof(AppAuthFlowMetadata).Assembly.GetManifestResourceNames().FirstOrDefault(n => n.Contains("client_secret.json"));
            var clientSecretStream = typeof(AppAuthFlowMetadata).Assembly.GetManifestResourceStream(name);
            flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecretsStream = clientSecretStream,
                Scopes = new[] { GmailService.Scope.GmailReadonly },
                DataStore = new FileDataStore("IPST_Web.Store")
            });
        }
        public override string GetUserId(Controller controller)
        {
            return controller.User.Identity.GetUserName();
        }

        public override IAuthorizationCodeFlow Flow
        {
            get { return flow; }
        }
    }
}