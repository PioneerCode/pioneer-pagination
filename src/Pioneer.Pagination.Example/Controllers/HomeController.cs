using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pioneer.Pagination.Example.Models;

namespace Pioneer.Pagination.Example.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPaginatedMetaService _paginatedMetaService;

        public HomeController(ILogger<HomeController> logger, IPaginatedMetaService paginatedMetaService)
        {
            _logger = logger;
            _paginatedMetaService = paginatedMetaService;
        }

        public IActionResult Index()
        {

            return View(new HomeViewModel
            {
                Start = _paginatedMetaService.GetMetaData(100, 2, 4),
                Full = _paginatedMetaService.GetMetaData(100, 5, 4),
                End = _paginatedMetaService.GetMetaData(100, 25, 4),
                Subset = _paginatedMetaService.GetMetaData(3, 2, 1),
                Zero = _paginatedMetaService.GetMetaData(0, 0, 1)
            });
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
