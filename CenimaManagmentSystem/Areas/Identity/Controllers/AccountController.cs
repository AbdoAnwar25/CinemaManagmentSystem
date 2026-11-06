using CenimaManagmentSystem.Models;
using CenimaManagmentSystem.Repository;
using CenimaManagmentSystem.ViewModels;
using Ecommerce533.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CenimaManagmentSystem.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _SignInManager;
        private readonly IRepository<ApplicationUserOtp> _applicationUserOtpRepository;
        private readonly IEmailSender _emailSender;


        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> SignInManager, IEmailSender emailSender, IRepository<ApplicationUserOtp> applicationUserOtpRepository)
        {
            _userManager = userManager;
            _SignInManager = SignInManager;
            _emailSender = emailSender;
            _applicationUserOtpRepository = applicationUserOtpRepository;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View(registerVM);
            }
            ApplicationUser user = new ApplicationUser()
            {
                Name = registerVM.Name,
                Address = registerVM.Address,
                UserName = registerVM.UserName,
                Email = registerVM.Email,
            };
            var result = await _userManager.CreateAsync(user, registerVM.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(registerVM);
            }

            // Confirmation Email
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var link = Url.Action(nameof(ConfirmEmail), "Account", new { area = "Identity", id = user.Id, token }, Request.Scheme);
            await _emailSender.SendEmailAsync(
                registerVM.Email,
                "ECommerce533 Confiramtion Email",
                $"<h1> please click <a href='{link}'> here </a>to confirm Your Mail</h1>"
                );


            return RedirectToAction("Login");
        }


        public async Task<IActionResult> ConfirmEmail(string id, string token)
        {
            var user = await _userManager.FindByIdAsync(id);

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                TempData["Error_Notification"] = "invalid Confirmation Email ";
                return RedirectToAction(nameof(Login));
            }
            TempData["Successful_Notification"] = "Confirm email Successfully";
            return RedirectToAction(nameof(Login));
        }



        [HttpGet]
        public IActionResult ResendEmailConfirmation()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResendEmailConfirmation(ResendEmailConfirmationVM resendEmailConfirmationVM)
        {
            var user = await _userManager.FindByEmailAsync(resendEmailConfirmationVM.UserNameOrEmail) ??
                        await _userManager.FindByNameAsync(resendEmailConfirmationVM.UserNameOrEmail);
            if (user == null)
            {
                ModelState.AddModelError("", "invalid userNameOr Email or Password");
                return View(resendEmailConfirmationVM);
            }
            if (user.EmailConfirmed)
            {
                ModelState.AddModelError("", "email already confirmed");
                return View(resendEmailConfirmationVM);
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var link = Url.Action(nameof(ConfirmEmail), "Account", new { area = "Identity", id = user.Id, token }, Request.Scheme);
            await _emailSender.SendEmailAsync(
                user.Email,
                "ECommerce533 Confiramtion Email",
                $"<h1> please click <a href='{link}'> here </a>to confirm Your Mail</h1>"
                );

            return RedirectToAction(nameof(Login));
        }











        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            var user = await _userManager.FindByEmailAsync(loginVM.UserNameOrEmail) ??
                         await _userManager.FindByNameAsync(loginVM.UserNameOrEmail);
            if (user == null)
            {
                ModelState.AddModelError("", "invalid userNameOr Email or Password");
                return View(loginVM);
            }
            var result = await _SignInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.RememberMe, true);

            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "to many attempts pls try again later");
                }
                else if (result.IsNotAllowed)
                {
                    ModelState.AddModelError("", "pleasae confirm Your Email First");

                }
                else
                {
                    ModelState.AddModelError("", "invalid userNameOr Email or Password");
                }
                return View(loginVM);
            }

            // عشان التوستر دي تظهر شوف الفديو ال4 من المحاضره 15 الدقيقه ال 24
            TempData["Successful_Notification"] = "Login Successfully";

            // السطر ده بيخليني بعد م اعمل لوجين يدخلني علي صفحه الافلام
            return RedirectToAction("Index", "Movie", new { area = "Admin" });
        }



        // forgetpasswor - send otp - reset password

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordVM forgetPasswordVM)
        {
            var user = await _userManager.FindByEmailAsync(forgetPasswordVM.UserNameOrEmail) ??
                        await _userManager.FindByNameAsync(forgetPasswordVM.UserNameOrEmail);
            if (user == null)
            {
                ModelState.AddModelError("", "invalid userNameOr Email or Password");
                return View(forgetPasswordVM);
            }
            var otp = new Random().Next(1000, 9999).ToString();
            var applicationuserOtp = new ApplicationUserOtp(otp, user.Id);

            await _applicationUserOtpRepository.AddAsync(applicationuserOtp);
            await _applicationUserOtpRepository.CommitAsync();

            await _emailSender.SendEmailAsync(
                user.Email,
                "ECommerce533 Reset Password",
                $"<h1> use this {otp} to Reset Your Password</h1>"
                );

            return RedirectToAction(nameof(VerifyOtp), new { userId = user.Id });
        }
        [HttpGet]
        public IActionResult VerifyOtp(string userId)
        {
            return View(new VerifyOtpVM() { UserId = userId });
        }

        [HttpPost]
        public async Task<IActionResult> VerifyOtp(VerifyOtpVM verifyOtpVM)
        {
            var user = await _userManager.FindByIdAsync(verifyOtpVM.UserId);
            if (user == null)
            {
                ModelState.AddModelError("", "invalid user");
                return View(verifyOtpVM);
            }
            var applicationUserOtps = await _applicationUserOtpRepository.GetListAsync(a =>
                a.ApplicationUserId == user.Id &&
                a.IsValid &&
                a.ValidTo >= DateTime.UtcNow
            );
            var applicationUserOtp = applicationUserOtps.OrderBy(a => a.CreatedAt).LastOrDefault();

            if (applicationUserOtp == null || applicationUserOtp.OTP != verifyOtpVM.OTP)
            {
                ModelState.AddModelError("", "invalid OTP / Expired");
                return View(verifyOtpVM);
            }
            applicationUserOtp.IsValid = false;
            await _applicationUserOtpRepository.CommitAsync();
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return RedirectToAction(nameof(ResetPassword), new { userId = user.Id, token });
        }
        [HttpGet]
        public IActionResult ResetPassword(string userId, string token)
        {
            return View(new ResetPasswordVM() { UserId = userId, Token = token });
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM resetPasswordVM)
        {
            if (!ModelState.IsValid)
            {
                return View(resetPasswordVM);
            }
            var user = await _userManager.FindByIdAsync(resetPasswordVM.UserId);
            if (user == null)
            {
                ModelState.AddModelError("", "invalid user");
                return View(resetPasswordVM);
            }
            var result = await _userManager.ResetPasswordAsync(user, resetPasswordVM.Token, resetPasswordVM.NewPassword);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(resetPasswordVM);
            }

            TempData["Successful_Notification"] = "Reset Password Successfully";
            return RedirectToAction(nameof(Login));
        }











    }
}
