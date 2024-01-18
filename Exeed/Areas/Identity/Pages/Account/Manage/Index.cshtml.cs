#nullable disable

using Exeed.Managers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Exeed.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly AccountManager _accountManager;
        private readonly DesireManager _desireManager;

        public IndexModel(
            IWebHostEnvironment appEnvironment,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            AccountManager accountManager,
            DesireManager desireManager)
        {
            _appEnvironment = appEnvironment;
            _userManager = userManager;
            _signInManager = signInManager;
            _accountManager = accountManager;
            _desireManager = desireManager;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Номер телефона")]
            public string PhoneNumber { get; set; }

            public IFormFile Photo { get; set; }
        }

        private async Task LoadAsync(IdentityUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return LocalRedirect("~/");
            }

            var account = await _accountManager.GetAsync(User);
            if (account?.Desire == null)
                return LocalRedirect("~/Identity/Account/Manage/Desire");

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostLogout()
        {
            await _signInManager.SignOutAsync();
            return LocalRedirect("~/");
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
