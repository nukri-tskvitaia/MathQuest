using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class Exercise : BaseEntity
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string Question { get; set; } = string.Empty;

        [Required]
        public IList<string> PossibleAnswers { get; set; } = [];

        [Required]
        public string CorrectAnswer { get; set; } = string.Empty;

        [Required]
        public int DifficultyLevel { get; set; }

        [Required]
        public int GradeId { get; set; }

        public Grade? Grade { get; }

        public ICollection<ExerciseRating>? ExerciseRatings { get; }

        public ICollection<UserExercise>? UserExercises { get; }
    }
}
