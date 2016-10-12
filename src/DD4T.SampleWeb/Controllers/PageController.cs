using DD4T.ContentModel;
using DD4T.ContentModel.Contracts.Resolvers;
using DD4T.ContentModel.Factories;
using DD4T.SampleWeb.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860
namespace DD4T.SampleWeb.Controllers
{
    public class PageController : Controller
    {
        private readonly IPublicationResolver _pub;

        private readonly MyDD4TConfiguration _config;

        public PageController(IOptions<MyDD4TConfiguration> settings)
        {
            _config = settings.Value;
        }

        //GET: /<controller>/

        //public IActionResult Index(string page)
        //{
        //    return View("Plain", page);
        //}

        public IActionResult Index(IHome page)
        {
            return View("ViewModel", page);
        }
    }
}