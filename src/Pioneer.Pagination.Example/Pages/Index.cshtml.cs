using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Pioneer.Pagination.Example.Pages
{
    public class IndexModel : PageModel
    {
        public PaginatedMetaModel Start { get; set; }
        public PaginatedMetaModel Full { get; set; }
        public PaginatedMetaModel End { get; set; }
        public PaginatedMetaModel Subset { get; set; }
        public PaginatedMetaModel Zero { get; set; }

        private readonly IPaginatedMetaService _paginatedMetaService;

        public IndexModel(IPaginatedMetaService paginatedMetaService)
        {
            _paginatedMetaService = paginatedMetaService;
        }

        public void OnGet()
        {
            Start = _paginatedMetaService.GetMetaData(100, 2, 4);
            Full = _paginatedMetaService.GetMetaData(100, 5, 4);
            End = _paginatedMetaService.GetMetaData(100, 25, 4);
            Subset = _paginatedMetaService.GetMetaData(3, 2, 1);
            Zero = _paginatedMetaService.GetMetaData(0, 0, 1);
        }
    }
}
