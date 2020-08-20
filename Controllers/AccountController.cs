using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using EmployeeManagement.Contracts.Request;
using EmployeeManagement.Datebase;
using EmployeeManagement.Service;
using EmployeeManagement.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;


namespace EmployeeManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        
        private readonly List<string> _countries;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly EmployeeManagerContext _managerContext;

        public AccountController(EmployeeManagerContext managerContext, IConfiguration config, RoleManager<IdentityRole> roleManager, IAccountService accountService)
        {
            _roleManager = roleManager;
            _accountService = accountService;
            _managerContext = managerContext;

            HttpClient client = new HttpClient();
            var data = client.GetAsync(config["Countries"]).Result.Content.ReadAsStringAsync().Result;
            _countries = JsonConvert.DeserializeObject<Dictionary<string, string>>(data).Values.ToList();

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
        public IActionResult Register()
        {
            UserRegistrationRequest model = new UserRegistrationRequest()
            {
                Id = Guid.NewGuid().ToString(),
                Countries = _countries
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


        public async Task<IActionResult> Profile()
        {
            IdentityEmployee x = new IdentityEmployee();
            return View(x);
        }

    }
}