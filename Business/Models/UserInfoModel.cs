using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public enum EmailExists
    {
        None,
        Exists
    }
    public class UserInfoModel
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; } = DateTime.Now;

        [Required]
        public string Gender { get; set; } = string.Empty;
    }
}
