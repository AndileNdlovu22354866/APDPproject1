using Microsoft.AspNet.Identity.Owin;
using Static_and_Login.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static Static_and_Login.IdentityConfig;
using static Static_and_Login.Models.RegisterViewModel;
using Microsoft.Owin;
using Static_and_Login.Services.Models;

namespace Static_and_Login.Controllers
{
  
        public class AccountController : Controller
        {
            private ApplicationSignInManager _signInManager;
            private ApplicationUserManager _userManager;

            private ApplicationDbContext db = new ApplicationDbContext();
            private EmailService emailService = new EmailService();

        public ApplicationSignInManager SignInManager
            {
                get => _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
                private set => _signInManager = value;
            }

            public ApplicationUserManager UserManager
            {
                get => _userManager ?? HttpContext.GetOwinContext().Get<ApplicationUserManager>();
                private set => _userManager = value;
            }
            

            [AllowAnonymous]
            public ActionResult Register()
            {
                return View();
            }

            [HttpPost]
            [AllowAnonymous]
            [ValidateAntiForgeryToken]
            public async Task<ActionResult> Register(RegisterViewModel model)
            {
                if (ModelState.IsValid)
                {
                    var user = new ApplicationUser
                    {
                        UserName = model.Username,
                        Email = model.Email,
                        Name = model.Name,
                        Surname = model.Surname,
                        CellNumber = model.CellNumber,
                        HomeAddress = model.HomeAddress,
                        Town = model.Town,
                        Province = model.Province,
                        Gender = model.Gender,
                        DateOfBirth = model.DateOfBirth
                    };

                    var result = await UserManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToAction("Index", "Home");
                    }
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        TempData["SuccessMessage"] = "Account created successfully! Welcome.";
                        return RedirectToAction("Index", "Home");
                    }

                foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                return View(model);
            }
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await SignInManager.PasswordSignInAsync(
                model.Username, model.Password, model.RememberMe, shouldLockout: false);

            switch (result)
            {
                case SignInStatus.Success:
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Home");

                case SignInStatus.LockedOut:
                    ModelState.AddModelError("", "This account has been locked out. Try again later.");
                    return View(model);

                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid username or password.");
                    return View(model);
            }
        }
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await UserManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                // Don't reveal that the user doesn't exist
                ModelState.AddModelError("", "If that email is registered, a code has been sent.");
                return View(model);
            }

            // Generate 6-digit OTP
            var random = new Random();
            string otpCode = random.Next(100000, 999999).ToString();

            var otp = new PasswordresetOtp
            {
                UserId = user.Id,
                OtpCode = otpCode,
                ExpiryTime = DateTime.UtcNow.AddMinutes(10),
                IsUsed = false,
                CreatedAt = DateTime.UtcNow
            };

            db.PasswordresetOtp.Add(otp);
            await db.SaveChangesAsync();

            await emailService.SendOtpEmailAsync(user.Email, otpCode);
            await emailService.SendOtpEmailAsync(user.Email, otpCode);

            TempData["SuccessMessage"] = "OTP sent successfully! Check your email.";
            return RedirectToAction("ResetPassword", new { email = model.Email });

            

        }

        [AllowAnonymous]
        public ActionResult ResetPassword(string email)
        {
            var model = new ResetPasswordViewModel { Email = email };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await UserManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid request.");
                return View(model);
            }

            var otp = db.PasswordresetOtp
                .Where(o => o.UserId == user.Id && o.OtpCode == model.OtpCode && !o.IsUsed)
                .OrderByDescending(o => o.CreatedAt)
                .FirstOrDefault();

            if (otp == null || otp.ExpiryTime < DateTime.UtcNow)
            {
                ModelState.AddModelError("", "Invalid or expired OTP code.");
                return View(model);
            }

            var removeResult = await UserManager.RemovePasswordAsync(user.Id);
            var addResult = await UserManager.AddPasswordAsync(user.Id, model.NewPassword);

            if (!addResult.Succeeded)
            {
                foreach (var error in addResult.Errors)
                {
                    ModelState.AddModelError("", error);
                }
                return View(model);
            }

            otp.IsUsed = true;
            await db.SaveChangesAsync();
            otp.IsUsed = true;
            await db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Password reset successful! Please log in.";
            return RedirectToAction("Login");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
    

}