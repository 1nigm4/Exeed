using Exeed.Data.Models;
using Exeed.Managers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exeed.Areas.Identity.Pages.Account.Manage
{
    public class DesiresModel : PageModel
    {
        public List<Desire> Desires { get; set; }
        private readonly AccountManager _accountManager;
        private readonly DesireManager _desireManager;
        private readonly LikeManager _likeManager;

        public DesiresModel(
            DesireManager desireManager,
            AccountManager accountManager,
            LikeManager likeManager)
        {
            _desireManager = desireManager;
            _accountManager = accountManager;
            _likeManager = likeManager;
            Desires = Enumerable.Empty<Desire>().ToList();
        }

        public async Task<IActionResult> OnGet()
        {
            var account = await _accountManager.GetAsync(User);
            var desires = await _desireManager.GetAsync();
            if (account == null)
                return LocalRedirect("~/Identity/Account/Login");
            Desires =  desires.Where(desire => desire != null && !desire.IsWon && ((account.Role == Role.User && desire.IsVerified == true) || (account.Role == Role.Moderator && !desire.IsVerified.HasValue && desire.PhotoPath != null))).ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostLike(string desireId)
        {
            var account = await _accountManager.GetAsync(User);
            var desire = await _desireManager.GetAsync(desireId);
            if (account == null || desire == null) return Redirect("Desires");
            var desireAcc = await _accountManager.GetAsync(desire);
            if (desireAcc == account) return Redirect("Desires");

            await _likeManager.LikeAsync(account, desire);
            return Redirect("Desires");
        }

        public async Task<IActionResult> OnGetVerify(string desireId, bool isVerified)
        {
            Request.QueryString = new QueryString();
            await _desireManager.VerifyAsync(desireId, isVerified);
            return Redirect("Desires");
        }
    }
}
