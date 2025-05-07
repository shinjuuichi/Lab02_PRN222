using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using ProductManagementRazorPages.SignalR;
using Services;

namespace ProductManagementRazorPages.Pages.Products
{
    public class CreateModel : PageModel
    {
        private readonly IProductService _contextProduct;
        private readonly ICategoryService _contextCategory;
        private readonly IHubContext<SignalRServer> _hubContext;

        public CreateModel(IProductService context, ICategoryService categoryService, IHubContext<SignalRServer> hubContext)
        {
            _contextProduct = context;
            _contextCategory = categoryService;
            _hubContext = hubContext;
        }

        public IActionResult OnGet()
        {
            ViewData["CategoryId"] = new SelectList(_contextCategory.GetCategories(), "CategoryId", "CategoryId");
            return Page();
        }

        [BindProperty]
        public Product Product { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            _contextProduct.SaveProduct(Product);
            await _hubContext.Clients.All.SendAsync("LoadAllItems");
            return RedirectToPage("./Index");
        }
    }
}
