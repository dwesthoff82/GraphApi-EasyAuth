using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Configuration;
using System.Security.Claims;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            try
            {
                var tenant = ConfigurationManager.AppSettings["Tenant"];
                var appKey = ConfigurationManager.AppSettings["AppKey"];
                var appClientID = ConfigurationManager.AppSettings["ClientId"];
                var targetResource = ConfigurationManager.AppSettings["TargetResource"];

                var userName = ClaimsPrincipal.Current.Identity.Name;

                UserAssertion userAssertion = new UserAssertion(this.Request.Headers["X-MS-TOKEN-AAD-ID-TOKEN"], "Bearer", userName);

                AuthenticationContext authenticationContext = new AuthenticationContext("https://login.windows.net/" + tenant);


                ClientCredential clientCredentials = new ClientCredential(appClientID, appKey);

                var result = authenticationContext.AcquireTokenAsync(targetResource, clientCredentials, userAssertion).Result;

                ViewBag.Message = result.UserInfo.DisplayableId + "Has been authorized for " + targetResource;
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.ToString();

            }
            return View();
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
    }
}