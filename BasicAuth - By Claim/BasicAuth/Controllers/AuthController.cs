using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using BasicAuth.Models;
using BasicAuth.Services;

namespace BasicAuth.Controllers
{
    [Route("auth")]
    public class AuthController : Controller
    {
        private IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [Route("signin")]
        public IActionResult SignIn()
        {
            return View(new SignInModel());
        }

        [Route("signin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(SignInModel model,string returnUrl = null)
        {
            if(ModelState.IsValid)
            {
                User user;
                if(await _userService.ValidateCredentials(model.Username,model.Password,out user))
                {
                    await SignInUser(user.Username);
                    if(returnUrl != null)
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
            }

            return View(model);
        }

        public async Task SignInUser(string username)
        {
            var claims = new List<Claim>();
            if(username == "Chris")
            {
                claims.Add(new Claim(ClaimTypes.NameIdentifier, username));
                claims.Add(new Claim("Gender", "Male"));
                claims.Add(new Claim(ClaimTypes.Name, username));
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.NameIdentifier, username));
                claims.Add(new Claim("Gender", "Female"));
                claims.Add(new Claim(ClaimTypes.Name, username));
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, null);
            var principal = new ClaimsPrincipal(identity);           
            await HttpContext.SignInAsync(principal);
        }

        [Route("signout")]
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}