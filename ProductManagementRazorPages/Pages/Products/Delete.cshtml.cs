using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using ProductManagementRazorPages.SignalR;
using Services;

namespace ProductManagementRazorPages.Pages.Products
{
    public class DeleteModel : PageModel
    {
        private readonly IProductService _contextProduct;
        private readonly IHubContext<SignalRServer> _hubContext;

        public DeleteModel(IProductService context, IHubContext<SignalRServer> hubContext)
        {
            _contextProduct = context;
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
            else
            {
                Product = product;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _contextProduct.GetProductById((int)id);
            if (product != null)
            {
                Product = product;
                _contextProduct.DeleteProduct(product);
                await _hubContext.Clients.All.SendAsync("LoadAllItems");
            }

            return RedirectToPage("./Index");
        }
    }
}
