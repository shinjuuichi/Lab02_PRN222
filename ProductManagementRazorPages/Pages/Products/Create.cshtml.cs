using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services;

namespace ProductManagementRazorPages.Pages.Products
{
    public class CreateModel : PageModel
    {
        private readonly IProductService _contextProduct;
        private readonly ICategoryService _contextCategory;


        public CreateModel(IProductService context, ICategoryService categoryService)
        {
            _contextProduct = context;
            _contextCategory = categoryService;
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

            return RedirectToPage("./Index");
        }
    }

}
