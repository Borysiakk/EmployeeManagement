using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.ViewModel
{
    public class LoginViewModel
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