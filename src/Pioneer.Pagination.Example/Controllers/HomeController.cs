using Microsoft.AspNetCore.Mvc;

namespace Pioneer.Pagination.Example.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPaginatedMetaService _paginatedMetaService;

        public HomeController(IPaginatedMetaService paginatedMetaService)
        {
            _paginatedMetaService = paginatedMetaService;
        }

        public IActionResult Index()
        {
            ViewBag.Start = _paginatedMetaService.GetMetaData(100, 2, 4);
            ViewBag.Full = _paginatedMetaService.GetMetaData(100, 5, 4);
            ViewBag.End = _paginatedMetaService.GetMetaData(100, 25, 4);
            ViewBag.Partial = _paginatedMetaService.GetMetaData(3, 2, 1);
            ViewBag.Zero = _paginatedMetaService.GetMetaData(0, 0, 1);
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
