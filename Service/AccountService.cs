using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using EmployeeManagement.Contracts.Request;
using EmployeeManagement.Contracts.Result;
using EmployeeManagement.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;

namespace EmployeeManagement.Service
{
    public class AccountService :IAccountService
    {
        private readonly IHostEnvironment _environment;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityEmployee> _userManager;
        private readonly SignInManager<IdentityEmployee> _signInManager;
        
        public AccountService(UserManager<IdentityEmployee> userManager, SignInManager<IdentityEmployee> signInManager, IHostEnvironment environment, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _environment = environment;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<UniversalResult> LoginAsync(UserLoginRequests request)
        {
            var result = await _signInManager.PasswordSignInAsync(request.User, request.Password, request.RememberMe, false);

            if (result.Succeeded)
            {
                return new UniversalResult()
                {
                    Success = true,
                };
            }
            else
            {
                return new UniversalResult()
                {
                    Success = false,
                    Error = new[] { "User/Password combination is wrong" }
                };
            }
        }

        public async Task<UniversalResult> RegisterAsync(UserRegistrationRequest request)
        {
            
            var existingUser = await _userManager.FindByEmailAsync(request.Email);

            if (existingUser != null)
            {
                return new UniversalResult()
                {
                    Success = false,
                    Error = new[] {"Adres email istnieje juz w bazie danych"}
                };
            }

            var user = new IdentityEmployee
            {
                IsFirstLogin = true,

                Age = request.Age,
                Name = request.Name,
                Email = request.Email,
                UserName = request.Email,
                Country = request.Country,
                Address = request.Address,
                PhoneNumber = request.Phone,
                LastName = request.LastName,
                Position = request.Position,
                ProfilePicture = request.Id + request.Photo.FileName.Substring(request.Photo.FileName.Length - 4, 4),
            };

            var result = await _userManager.CreateAsync(user, user.Email);

            if (result.Succeeded)
            {
                if (request.Photo != null)
                {
                    string uploadsFolder = Path.Combine(_environment.ContentRootPath, "Image");
                    string filePath = Path.Combine(uploadsFolder, user.ProfilePicture);

                    await using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await request.Photo.CopyToAsync(stream);
                    }
                }

                await _userManager.AddToRoleAsync(user, request.Permissions);
                
                return new UniversalResult() { Success = true };
            }

            return new UniversalResult()
            {
                Success = false,
                Error = result.Errors.Select(x => x.Description)
            };
        }
    }
}