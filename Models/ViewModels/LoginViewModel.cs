using System.ComponentModel.DataAnnotations;

namespace MMS.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter an email address.")]
        public string? Email { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide a password.")]
        public string? Password { get; set; }
    }
}
