using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace EmployeeManagement.Contracts.Request
{
    public class UserRegistrationRequest
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
        [Required]
        public string Permissions { get; set; }
        public IFormFile Photo { get; set; }
        public IEnumerable<string> Countries { get; set; }
    }
}