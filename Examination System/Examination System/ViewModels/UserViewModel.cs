using Examination_System.Repos.Login;
using System.ComponentModel.DataAnnotations;

namespace Examination_System.ViewModels
{
    public class UserViewModel
    {
        [RegularExpression("^[0-9]*$", ErrorMessage = "Please enter a valid ID")]
        [MinLength(14, ErrorMessage = "Please enter a valid ID")]
        [MaxLength(14, ErrorMessage = "Please enter a valid ID")]
        [Display(Name = "National ID")]
        [Required(ErrorMessage = "Please enter your ID")]
        public string Id { get; set; }

        [Required(ErrorMessage = "Please enter your password")]
        public string Password { get; set; }
        public string Role { get; set; }
        public string Name { get; set; }
        
    }
}
