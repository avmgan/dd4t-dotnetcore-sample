using DD4T.ContentModel.Contracts.Logging;
using Microsoft.AspNetCore.Mvc;
using WTF4T.Modules.Navigation.Models;

namespace WTF4T.Modules.Navigation.Controllers
{
    [Area("Navigation")]
    public class NavigationController : Controller
    {
        public virtual ActionResult Navigation(ISitemapItem model, string view)
        {
            return View(view ?? "TopNavigation", model);
        }
    }
}