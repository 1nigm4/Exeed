using Exeed.Managers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
#nullable disable
namespace Exeed.Areas.Identity.Pages.Account.Manage
{
    public class StarModel : PageModel
    {
        [BindProperty]
        public InputModel Input { get; set; }
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly AccountManager _accountManager;
        private readonly DesireManager _desireManager;

        public StarModel(IWebHostEnvironment appEnvironment, AccountManager accountManager, DesireManager desireManager)
        {
            _appEnvironment = appEnvironment;
            _accountManager = accountManager;
            _desireManager = desireManager;
        }

        public class InputModel
        {
            public IFormFile Photo { get; set; }
        }

        public async Task<IActionResult> OnGet()
        {
            var account = await _accountManager.GetAsync(User);
            if (account?.Desire == null)
                return LocalRedirect("~/Identity/Account/Manage/Desire");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Input.Photo == null) return Page();

            var account = await _accountManager.GetAsync(User);
            string path = $"/Desires/{account.Desire.Id}.{Input.Photo.FileName.Split('.').Last()}";
            using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
            {
                await Input.Photo.CopyToAsync(fileStream);
            }

            await _desireManager.UploadPhoto(account.Desire, path);

            return LocalRedirect("~/Identity/Account/Manage/Desires");
        }
    }
}
