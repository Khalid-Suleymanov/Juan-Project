using System.ComponentModel.DataAnnotations;

namespace BackendProject.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required]
        [MaxLength(30)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [MaxLength(45)]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Token { get; set; }
    }
}
