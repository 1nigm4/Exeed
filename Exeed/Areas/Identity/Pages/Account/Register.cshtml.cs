#nullable disable

using Exeed.Managers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;

namespace Exeed.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly AccountManager _accountManager;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            AccountManager accountManager)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = (IUserEmailStore<IdentityUser>)_userStore;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _accountManager = accountManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "FirstName")]
            [RegularExpression("[a-zA-Zа-яёЁА-Я]+", ErrorMessage = "Имя должно содержать только буквы")]
            public string FirstName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.PhoneNumber)]
            [Display(Name = "PhoneNumber")]
            public string PhoneNumber { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "Длина пароля от {2} символов", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = Activator.CreateInstance<IdentityUser>();

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                var errors = result.Errors;
                if (result.Succeeded)
                {
                    var phoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                    errors = errors.Concat(phoneResult.Errors);
                    if (phoneResult.Succeeded)
                    {
                        bool isSuccessed = await _accountManager.RegisterAsync(user, Input.FirstName);
                        
                        if (isSuccessed)
                        {
                            _logger.LogInformation("User created a new account with password.");

                            var userId = await _userManager.GetUserIdAsync(user);
                            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                            string callbackUrl = $"https://exeed2024.ru/Identity/Account/ConfirmEmail?userId={userId}&code={code}&returnUrl={returnUrl}";

                            await _emailSender.SendEmailAsync(Input.Email, "Подтверждение регистрации",
                                $"Пожалуйста подтвердите регистрацию перейдя по <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>ссылке</a>.");

                            if (_userManager.Options.SignIn.RequireConfirmedAccount)
                            {
                                return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                            }
                            else
                            {
                                await _signInManager.SignInAsync(user, isPersistent: false);
                                return LocalRedirect(returnUrl);
                            }
                        }
                    }
                }
                foreach (var error in errors)
                {
                    string description = error.Description;
                    if (error.Code == "DuplicateUserName")
                        description = "Почта уже зарегистрирована";
                    ModelState.AddModelError(string.Empty, description);
                }
            }

            return Page();
        }
    }
}
