using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class Theory : BaseEntity
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        [Required]
        public int GradeId { get; set; }

        public Grade? Grade { get; }

        public ICollection<TheoryRating>? TheoryRatings { get; }

        public ICollection<UserTheory>? UserTheories { get; }
    }
}
