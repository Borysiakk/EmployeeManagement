using System;
using System.Linq;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using EmployeeManagement.Datebase;
using EmployeeManagement.Service;
using EmployeeManagement.ViewModel;
using Microsoft.Extensions.Configuration;
using EmployeeManagement.Contracts.Request;
using Microsoft.AspNetCore.Authorization;


namespace EmployeeManagement.Controllers
{
    public class AccountController : Controller
    {
        
        private readonly IEnumerable<string> _countries;
        private readonly IAccountService _accountService;
        private readonly EmployeeManagerContext _managerContext;

        public AccountController(EmployeeManagerContext managerContext, IConfiguration config, IAccountService accountService)
        {
            _accountService = accountService;
            _managerContext = managerContext;

            HttpClient client = new HttpClient();
            var data = client.GetAsync(config["Countries"]).Result.Content.ReadAsStringAsync().Result;
            _countries = JsonConvert.DeserializeObject<Dictionary<string, string>>(data).Values;
            
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View("_Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginRequests model)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountService.LoginAsync(model);
                
                if (result.Success) return Json("OK");

                foreach (var error in result.Error)
                {
                    ModelState.AddModelError(string.Empty,error);
                }
            }

            return Json(new 
            {
                succes = false,
                errors = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)),
            });
        }

        [HttpGet]
        //[Authorize(Roles = "Administrator")]
        public IActionResult Register()
        {
            UserRegistrationRequest model = new UserRegistrationRequest()
            {
                Countries = _countries,
                Id = Guid.NewGuid().ToString(),
            };
            
            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegistrationRequest model)
        {
            if (ModelState.IsValid)
            {
               var result = await _accountService.RegisterAsync(model);
               
               if (result.Success)
               {
                   return RedirectToAction("index", "Home");
               }
               
               foreach (var error in result.Error)
               {
                   ModelState.AddModelError(string.Empty,error);
               }
            }
            model.Countries = _countries;
            return View(model);
            
        }

        public async Task<IActionResult> Logout()
        {
            await _accountService.LogoutAsync();
            return RedirectToAction("Index","Home");
        }


        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Profile()
        {
            IdentityEmployee x = new IdentityEmployee();
            return View(x);
        }

    }
}