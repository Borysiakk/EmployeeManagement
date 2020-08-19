using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


using EmployeeManagement.ViewModel;

namespace EmployeeManagement.Datebase
{
    public class EmployeeManagerContext : IdentityDbContext
    {

        public DbSet<Competition> Competition { get; set; }
        public DbSet<IdentityEmployee> Employee { get; set; }
        public EmployeeManagerContext(DbContextOptions<EmployeeManagerContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

    }
}
