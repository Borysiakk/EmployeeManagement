using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace EmployeeManagement.ViewModel
{
    public class RegisterViewModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public DateTime  Age { get; set; }
        [Required]
        public string Position  { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        public IFormFile Photo { get; set; }
        public List<string> Countries { get; set; }
    }
}