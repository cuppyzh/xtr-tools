using cuppyzh.xtrtools.poadocumentgenerator.Exceptions;
using cuppyzh.xtrtools.poadocumentgenerator.Models;
using cuppyzh.xtrtools.poadocumentgenerator.Services.Interfaces;
using cuppyzh.xtrtools.poadocumentgenerator.Utilities;
using cuppyzh.xtrtools.poadocumentgenerator.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace cuppyzh.xtrtools.poadocumentgenerator.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserServices _userServices;

        public HomeController(ILogger<HomeController> logger, IUserServices userServices)
        {
            _logger = logger;
            _userServices = userServices;
        }

        public IActionResult Index()
        {
            try
            {
                ViewBag.GitPrlUrlPreview = UrlUtils.GetPrUrlPreview();
                ViewBag.IsAuthenticated = _userServices.IsAuthenticated();
            }
            catch (XtoolsException ex)
            {
                _logger.LogException(ex);
                ViewBag.ErrorMessage = ex.Message;
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                ViewBag.ErrorMessage = "Error 500: Internal Server Error";
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}