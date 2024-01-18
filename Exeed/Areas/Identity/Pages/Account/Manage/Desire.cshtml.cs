using Exeed.Managers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Exeed.Areas.Identity.Pages.Account.Manage
{
    public class DesireModel : PageModel
    {
        [BindProperty]
        [StringLength(75, MinimumLength = 5)]
        public string Text { get; set; }
        private readonly AccountManager _accountManager;
        private readonly DesireManager _desireManager;

        public DesireModel(AccountManager accountManager, DesireManager desireManager)
        {
            _accountManager = accountManager;
            _desireManager = desireManager;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var account = await _accountManager.GetAsync(User);
            if (account == null) return LocalRedirect("~/Identity/Account/Login");
            if (account.Desire != null) return LocalRedirect("~/Identity/Account/Manage/Index");

            var desire = await _desireManager.NewWish(Text);
            if (desire == null) return Page();
            await _accountManager.AddDesire(account, desire);
            return LocalRedirect("~/Identity/Account/Manage/Star");
        }
    }
}
