using Data.Data;

namespace Data.Repositories
{
    public abstract class AbstractRepository
    {
        internal readonly MathQuestDbContext _context;

        protected AbstractRepository(MathQuestDbContext context)
        {
            this._context = context;
        }
    }
}
