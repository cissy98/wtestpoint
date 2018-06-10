using System.Web.Mvc;

namespace RewardsAPI.Controllers
{
    public class HomeController : Controller
    {
        public ContentResult index()
        {
            return Content("ok");
        }
    }
}
