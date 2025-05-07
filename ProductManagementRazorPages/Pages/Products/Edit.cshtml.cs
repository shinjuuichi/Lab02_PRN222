using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ProductManagementRazorPages.SignalR;
using Services;

namespace ProductManagementRazorPages.Pages.Products
{
    public class EditModel : PageModel
    {
        private readonly IProductService _contextProduct;
        private readonly ICategoryService _contextCategory;
        private readonly IHubContext<SignalRServer> _hubContext;

        public EditModel(IProductService context, ICategoryService categoryService, IHubContext<SignalRServer> hubContext)
        {
            _contextProduct = context;
            _contextCategory = categoryService;
            _hubContext = hubContext;
        }

        [BindProperty]
        public Product Product { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _contextProduct.GetProductById((int)id);
            if (product == null)
            {
                return NotFound();
            }
            Product = product;
            ViewData["CategoryId"] = new SelectList(_contextCategory.GetCategories(), "CategoryId", "CategoryId");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                _contextProduct.UpdateProduct(Product);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(Product.ProductId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            await _hubContext.Clients.All.SendAsync("LoadAllItems");
            return RedirectToPage("./Index");
        }

        private bool ProductExists(int id)
        {
            return (_contextProduct.GetProductById(id) == null) ? true : false;
        }
    }
}
