using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.Gmail.v1;
using Google.Apis.Http;
using Google.Apis.Services;
using IPST_Engine;
using IPST_Web.Models;

namespace IPST_Web.Controllers
{
    
    public class HomeController : Controller
    {
        public HomeController(IIPSTEngine ipstEngine)
        {
            
        }
        [Authorize]
        public ActionResult Index()
        {
            
            return View(new HomeModel());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [Authorize]
        public ActionResult Login()
        {
            ViewBag.Message = "Login to Google.";

            return View();
        }

        [Authorize]
        public async Task<ActionResult> CheckEmailAsync(CancellationToken cancellationToken)
        {
            var authorizationCodeMvcApp = new AuthorizationCodeMvcApp(this, new AppAuthFlowMetadata());
            var result = await authorizationCodeMvcApp.
                    AuthorizeAsync(cancellationToken);

            if (result.Credential == null)
                return new RedirectResult(result.RedirectUri);


            return Redirect(Request.UrlReferrer.ToString());
        }
    }
}