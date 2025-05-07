using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;

namespace ProductManagementRazorPages.Pages
{
    public class LoginModel : PageModel
    {

        private IAccountService _accountService;
        public LoginModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var loginId = HttpContext.Session.GetInt32("Account").ToString();
            if (!string.IsNullOrEmpty(loginId))
            {
                return RedirectToPage("/Products/Index");
            }
            return Page();
        }

        [BindProperty]
        public AccountMember AccountMember { get; set; } = default!;
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var loginId = HttpContext.Session.GetInt32("Account").ToString();
            if (!string.IsNullOrEmpty(loginId))
            {
                return RedirectToPage("/Products/Index");
            }

            var memberAccount = _accountService.GetAccountByEmail(AccountMember.EmailAddress);


            if (memberAccount == null || memberAccount.MemberPassword != AccountMember.MemberPassword)
            {
                ErrorMessage = "You do not have permission to do this function!";
                ModelState.AddModelError(string.Empty, ErrorMessage);

                return Page();
            }
            else if (memberAccount.MemberRole == 1 || memberAccount.MemberRole == 2)
            {
                HttpContext.Session.SetInt32("Account", memberAccount.MemberRole);
                return RedirectToPage("/Products/Index");
            }
            else
            {
                ErrorMessage = "You do not have permission to do this function!";
                ModelState.AddModelError(string.Empty, ErrorMessage);
                return Page();
            }
        }

    }
}
