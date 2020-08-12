using EmployeeManagement.Datebase;
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
            services.AddDbContext<EmployeeManagerContex>(options => options.UseSqlServer(Configuration.GetConnectionString("EmployeeManagmentDb")));
            services.AddIdentity<IdentityUser, IdentityRole>(option =>
            {
                option.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<EmployeeManagerContex>();
        }
    }
}
