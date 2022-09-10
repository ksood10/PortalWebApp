using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using PortalWebApp.Areas.Identity.Data;
using PortalWebApp.Models;
using PortalWebApp.Utilities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static PortalWebApp.Utilities.Util;

namespace PortalWebApp.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<PortalWebAppUser> _userManager;
        private readonly SignInManager<PortalWebAppUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
         public static PortalWebApp.Data.PortalWebAppContext _databasecontext { get; set; }
        public LoginModel(SignInManager<PortalWebAppUser> signInManager, 
            ILogger<LoginModel> logger,
            UserManager<PortalWebAppUser> userManager, PortalWebApp.Data.PortalWebAppContext databasecontext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _databasecontext = databasecontext;
        }

        [Required(ErrorMessage = "Required")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Must be numeric")]
        public int UserID { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "User Name")]
            public string UserName { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }
      
        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }


        public IActionResult OnPost(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                // var result = await _signInManager.PasswordSignInAsync(Input.UserName, Input.Password,isPersistent: false, lockoutOnFailure: false);
                var userid = ValidateUser(Env.Dev.Value, Input.UserName);
                if(userid == -1)
                {
                    ModelState.AddModelError(string.Empty, "Invalid User ID ");
                    return Page();
                }
                var password = GetPassword(Env.Dev.Value, userid);
                
                if (password == Input.Password)
                {
                    var userDB = LoginModel._databasecontext.User.Where(u => u.UserId == userid).FirstOrDefault();
                    TempData["LoginCheck"]="LoggedIn";
                    TempData["Username"] =  userDB.AbbreviatedName;
                  
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect("/Home/BulkConfig");
                }

                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt. Wrong password for this user");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
