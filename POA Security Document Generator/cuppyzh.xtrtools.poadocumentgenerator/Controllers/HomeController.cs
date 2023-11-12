using cuppyzh.xtrtools.poadocumentgenerator.Models;
using cuppyzh.xtrtools.poadocumentgenerator.Services;
using cuppyzh.xtrtools.poadocumentgenerator.Utilities;
using cuppyzh.xtrtools.poadocumentgenerator.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace cuppyzh.xtrtools.poadocumentgenerator.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserServices _userServices = new UserServices();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.GitPrlUrlPreview = UrlUtils.GetPrUrlPreview();
            ViewBag.IsAuthenticated = _userServices.IsAuthenticated();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}