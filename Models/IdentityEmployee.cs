using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.ViewModel
{
    public class IdentityEmployee : IdentityUser
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime Age { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string Position  { get; set; }
        public string ProfilePicture  { get; set; }
        public bool IsFirstLogin { get; set; }
    }
}
