using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Web.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using WhiteLagoon.Application.Utility;
namespace WhiteLagoon.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Login(string returnUrl = null)
        {
            returnUrl??= Url.Content("~/");
            LoginVm loginVm = new()
            {
                RedirectUrl = returnUrl
            };

            return View(loginVm);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVm loginVm)
        {
            if(ModelState.IsValid)
            {
                var result = await _signInManager
                    .PasswordSignInAsync(loginVm.Email, loginVm.Password, loginVm.RememberMe, lockoutOnFailure: false);

                if(result.Succeeded)
                {
                    if (string.IsNullOrEmpty(loginVm.RedirectUrl))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return LocalRedirect(loginVm.RedirectUrl);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                }

            }
            return View(loginVm);

        }

        public IActionResult Register(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            if(!_roleManager.RoleExistsAsync(Sd.Role_Admin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(Sd.Role_Admin)).Wait();
                _roleManager.CreateAsync(new IdentityRole(Sd.Role_Customer)).Wait();
            }

            RegisterVm registerVm = new()
            {
                RoleList = _roleManager.Roles.Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Name
                })
            };

            return View(registerVm);
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVm registerVm)
        {
            if(ModelState.IsValid)
            {
				ApplicationUser user = new()
				{
					Name = registerVm.Name,
					Email = registerVm.Email,
					PhoneNumber = registerVm.PhoneNumber,
					NormalizedEmail = registerVm.Email,
					EmailConfirmed = true,
					UserName = registerVm.Email,
					CreatedAt = DateTime.Now,
				};

				var result = await _userManager.CreateAsync(user, registerVm.Password);

				if (result.Succeeded)
				{
					if (!string.IsNullOrEmpty(registerVm.Role))
					{
						await _userManager.AddToRoleAsync(user, registerVm.Role);
					}
					else
					{
						await _userManager.AddToRoleAsync(user, Sd.Role_Customer);
					}

					await _signInManager.SignInAsync(user, isPersistent: false);

					if (string.IsNullOrEmpty(registerVm.RedirectUrl))
					{
						return RedirectToAction("Index", "Home");
					}
					else
					{
						return LocalRedirect(registerVm.RedirectUrl);
					}
				}

				foreach (var error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}
			}

            registerVm.RoleList = _roleManager.Roles.Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Name
            });

            return View(registerVm);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
