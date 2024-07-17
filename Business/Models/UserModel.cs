using Data.Entities;

namespace Business.Models
{
    public class UserModel
    {
        public string Id { get; set; } = string.Empty;

        public int PersonId { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public DateTime BirthDate { get; set; } = DateTime.Now;

        public string Gender {  get; set; } = string.Empty;

        public IEnumerable<Friend>? Friends { get; set; }
    }
}
