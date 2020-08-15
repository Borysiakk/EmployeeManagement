using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


using EmployeeManagement.ViewModel;

namespace EmployeeManagement.Datebase
{
    public class EmployeeManagerContex : IdentityDbContext
    {
        public DbSet<IdentityEmployee> Employee { get; set; }
        public EmployeeManagerContex(DbContextOptions<EmployeeManagerContex> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

    }
}
