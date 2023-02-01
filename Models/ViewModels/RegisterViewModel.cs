using System.ComponentModel.DataAnnotations;

namespace ShoppingSystem.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Password don`t match")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string PasswordConfirm { get; set; }
    }
}
