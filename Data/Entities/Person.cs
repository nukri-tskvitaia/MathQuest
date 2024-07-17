using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public enum GenderList
    {
        Male,
        Female,
        Neither
    }

    public class Person : BaseEntity
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        public GenderList Gender { get; set; } = GenderList.Neither;
    }
}
