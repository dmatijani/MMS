using System.ComponentModel.DataAnnotations;

namespace MMS.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Molimo unijeti email adresu.")]
        public string? Email { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Molimo unijeti lozinku.")]
        public string? Password { get; set; }
    }
}
