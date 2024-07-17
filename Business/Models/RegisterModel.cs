using Business.Validations;
using Data.Entities;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public class RegisterModel
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Required]
        public GenderList Gender { get; set; } = GenderList.Neither;

        [ImageValidator([".jpg", ".jpeg", ".png"])]
        public IFormFile? ProfilePicture { get; set; }

        public string? ProfilePictureUrl { get; set; }

        public string? PhoneNumber { get; set; } = string.Empty;

        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, MinLength(8), MaxLength(100)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match!")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
