using System.ComponentModel.DataAnnotations;

namespace LSTM.Models.DTOs
{
    public class RegisterDto
    {
        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
} 