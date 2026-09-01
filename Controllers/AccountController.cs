using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Next_Store.Models;
using Login = Next_Store.Models.Login;

namespace Next_Store.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IPasswordHasher<AppUser> _passwordHasher;
        public AccountController(UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager, IPasswordHasher<AppUser> _passwordHasher)
        {
           this._userManager = _userManager;         
           this._signInManager = _signInManager;
            this._passwordHasher = _passwordHasher;
        }
        [AllowAnonymous]
        // GET Account/Register
        [Route("/Account/Register")]

        public IActionResult Register() => View();
        // POST Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Route("/Account/Register")]

        public async Task<IActionResult> Register(User user)
        {
            if(ModelState.IsValid)
            {
                AppUser appUser = new AppUser
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Occupation="none"
                };
                IdentityResult result = await _userManager.CreateAsync(appUser, user.Password);
                if(result.Succeeded)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(user);
        }
        // GET Account/Login
        [AllowAnonymous]
        [Route("/Account/Login")]
        public IActionResult Login(string returnUrl) 
        {
            Login login = new Login
            {
                ReturnUrl=returnUrl
            };
            return View(login);
        }
        // POST Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Route("/Account/Login")]
        public async Task<IActionResult> Login(Login login)
        {
            if(ModelState.IsValid)
            {
                // check if user exists
                AppUser appUser = await _userManager.FindByEmailAsync(login.Email);
                if(appUser!=null)
                {
                    Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(appUser, login.Password, false, false);
                    if (result.Succeeded)
                    {
                        return Redirect(login.ReturnUrl ?? "/");

                    }
                    else
                    {
                        ModelState.AddModelError("", "Login Failed due to wrong credentails");
                        return View(login);
                    }

                    
                }

            }   
            return View(login);
        }
        // GET Account/Logout
        
        [Route("/Account/Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/Pages/Page/home");
        }
        // GET Account/Edit
        
        [Route("/Account/Edit")]
        public async Task<IActionResult> Edit()
        {
            AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);
            UserEdit user = new UserEdit(appUser);
            return View(user);
        }
        // POST Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/Account/Edit")]
        public async Task<IActionResult> Edit(UserEdit userEdit)
        {
            AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);

            if (ModelState.IsValid)
            {
                appUser.Email = userEdit.Email;
                if(userEdit.Password != null)
                {
                   appUser.PasswordHash = _passwordHasher.HashPassword(appUser, userEdit.Password);
                }
                IdentityResult result = await _userManager.UpdateAsync(appUser);
                if (result.Succeeded)
                {
                    TempData["Success"] = "Your account information has been updated";
                }
            }
            return View();
        }
    }
}
