using Data.Entities.Identity;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class UserExercise : BaseEntity
    {
        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public int ExerciseId { get; set; }

        public User? User { get; }

        public Exercise? Exercise { get; }
    }
}
