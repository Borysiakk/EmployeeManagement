using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Contracts.Request
{
    public class UserLoginRequests
    {
        [Required]
        public string User { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name="Zapamietaj mnie")]
        public bool RememberMe { get; set; }
    }
}