using Data.Entities.Identity;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class Grade : BaseEntity
    {
        [Required]
        public string GradeName { get; set; } = string.Empty;

        [Required]
        public int GradeLevel { get; set; }

        public ICollection<Exercise>? Grades { get; }

        public ICollection<Theory>? Theories { get; }

        public ICollection<User>? Users { get; }
    }
}
