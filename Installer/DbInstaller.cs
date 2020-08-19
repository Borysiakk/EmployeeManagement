using EmployeeManagement.Datebase;
using EmployeeManagement.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeManagement.Installer
{
    public class DbInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<EmployeeManagerContext>(options => options.UseSqlServer(Configuration.GetConnectionString("EmployeeManagmentDb")));
            services.AddIdentity<IdentityEmployee, IdentityRole>(option =>
            {
                option.Password.RequiredLength = 15;
                option.Password.RequireNonAlphanumeric = true;
                option.Password.RequireUppercase = false;
                option.Password.RequireLowercase = false;
                option.Password.RequireDigit = false;

            }).AddEntityFrameworkStores<EmployeeManagerContext>();


        }
    }
}
