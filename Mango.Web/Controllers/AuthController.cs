using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Reflection;

namespace Mango.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDto loginRequestDto = new();
            return View(loginRequestDto);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto obj)
        {
            var responseDto = await _authService.LoginAsync(obj);

            if (responseDto != null && responseDto.IsSuccess)
            {
                LoginResponseDto loginResponseDto = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(responseDto.Result));
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("CustomError", responseDto.Message);
                return View(obj);
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            var roleList = new List<SelectListItem>()
            {  
                new SelectListItem()
                { 
                    Text = Constants.RoleAdmin,
                    Value = Constants.RoleAdmin
                },
                new SelectListItem()
                {
                    Text = Constants.RoleCustomer,
                    Value = Constants.RoleCustomer
                }
            };

            ViewBag.RoleList = roleList;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequestDto obj)
        {
            var responseDto = await _authService.RegisterAsync(obj);
            ResponseDto assignRole;

            if(responseDto != null && responseDto.IsSuccess)
            {
                if(string.IsNullOrEmpty(obj.Role))
                {
                    obj.Role = Constants.RoleCustomer;
                }

                assignRole = await _authService.AssignRoleAsync(obj);

                if(assignRole != null && assignRole.IsSuccess) 
                {
                    TempData["success"] = "Registration Sucessful";
                    return RedirectToAction(nameof(Login));
                }
            }
            else
            {
                TempData["error"] = responseDto.Message;
            }


            var roleList = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text = Constants.RoleAdmin,
                    Value = Constants.RoleAdmin
                },
                new SelectListItem()
                {
                    Text = Constants.RoleCustomer,
                    Value = Constants.RoleCustomer
                }
            };

            ViewBag.RoleList = roleList;

            return View(obj);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            return View();
        }
    }
}
