using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.BusinessLogic.Implementation.Users;
using Restaurant.Entities.Enums;
using Restaurant.Web.Code.Base;
using Restaurant.Web.Code.Utils;

namespace Restaurant.Web.Controllers
{
    public class UserController : BaseController
    {
        private readonly UserService Service;
        public UserController(ControllerDependencies dependencies, UserService service) : base(dependencies)
        {
            Service = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            var model = new RegisterModel();

            return View("Register", model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            var user = await Service.RegisterUser(model);
            await LoginUtils.LogIn(user, HttpContext);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (CurrentUser.IsAuthenticated)
            {
                throw new AccessViolationException();
            }
            var model = new LoginModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var user = await Service.Login(model.Email, model.Password);

            if (!user.IsAuthenticated)
            {
                model.AreCredentialsInvalid = true;
                return View(model);
            }

            await LoginUtils.LogIn(user, HttpContext);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await LoginUtils.LogOut(HttpContext);

            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> BecomeManager()
        {
            await Service.BecomeManager();
            var user = CurrentUser;
            user.Role = RoleTypes.PendingManager.ToString();
            await LoginUtils.LogOut(HttpContext);
            await LoginUtils.LogIn(user, HttpContext);

            return Ok(new { success = true });
        }
    }
}
