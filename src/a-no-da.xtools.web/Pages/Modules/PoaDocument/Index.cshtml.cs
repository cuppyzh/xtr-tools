using Microsoft.AspNetCore.Mvc.RazorPages;

namespace a_no_da.xtools.web.Pages.Modules.PoaDocument
{
    public class Index : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public Index(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}
