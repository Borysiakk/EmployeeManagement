using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using EmployeeManagement.Datebase;
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
        
        private readonly List<string> _countries;
        private readonly IHostEnvironment _environment;
        private readonly EmployeeManagerContext _managerContext;
        private readonly UserManager<IdentityEmployee> _userManager;
        private readonly SignInManager<IdentityEmployee> _signInManager;

        public AccountController(UserManager<IdentityEmployee> userManager, SignInManager<IdentityEmployee> signInManager, EmployeeManagerContext managerContext, IConfiguration config, IHostEnvironment environment)
        {
            _environment = environment;
            _userManager = userManager;
            _signInManager = signInManager;
            _managerContext = managerContext;

            HttpClient client = new HttpClient();
            var data = client.GetAsync(config["Countries"]).Result.Content.ReadAsStringAsync().Result;
            _countries = JsonConvert.DeserializeObject<Dictionary<string, string>>(data).Values.ToList();

        }

        [HttpGet]
        public IActionResult Register()
        {
            RegisterViewModel model = new RegisterViewModel()
            {
                Id = Guid.NewGuid().ToString(),
                Countries = _countries
            };
            
            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                string fileName = model.Id + model.Photo.FileName.Substring(model.Photo.FileName.Length-4, 4);
                
                var user = new IdentityEmployee
                {
                    Age = model.Age,
                    Name = model.Name,
                    Email = model.Email,
                    UserName = model.Id,
                    Country = model.Country,
                    Address = model.Address,
                    PhoneNumber = model.Phone,
                    LastName = model.LastName,
                    Position = model.Position,
                    ProfilePicture = fileName,
                    IsFirstLogin = true
                };
                
                var result = await _userManager.CreateAsync(user,user.Email );
                if (result.Succeeded)
                {
                    if (model.Photo != null)
                    {
                        string uploadsFolder = Path.Combine(_environment.ContentRootPath, "Image");
                        string filePath = Path.Combine(uploadsFolder, fileName);
                        await using (var stream = new FileStream(filePath,FileMode.Create))
                        {
                            await model.Photo.CopyToAsync(stream);
                        }
                    }
                    return RedirectToAction("index", "Home");
                }
                
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty,error.Description);
                }
            }

            model.Countries = _countries;
            return View(model);
            
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View("_Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.User, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return Json("OK");
                }
                else
                {
                    return Json("Incorrect login or password!");
                }
            }
            else
            {
                return Json(new
                {
                    succes = false,
                    errors = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)),
                });
                
            }
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }


        public async Task<IActionResult> Profile()
        {
            IdentityEmployee x = await _userManager.GetUserAsync(User);
            return View(x);
        }

    }
}