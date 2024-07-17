using Data.Entities.Identity;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class UserTheory : BaseEntity
    {
        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public int TheoryId { get; set; }

        public User? User { get; }

        public Theory? Theory { get; }
    }
}
