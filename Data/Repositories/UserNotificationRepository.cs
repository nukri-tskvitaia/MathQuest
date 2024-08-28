using Data.Data;
using Data.Entities;
using Data.Entities.Identity;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class UserNotificationRepository : AbstractRepository, IUserNotificationRepository
    {
        private readonly DbSet<UserNotification> _userNotifications;

        public UserNotificationRepository(MathQuestDbContext context)
            : base(context)
        {
            this._userNotifications = context.Set<UserNotification>();
        }

        public Task AddAsync(UserNotification entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(UserNotification entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserNotification>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<UserNotification> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(UserNotification entity)
        {
            throw new NotImplementedException();
        }
    }
}
