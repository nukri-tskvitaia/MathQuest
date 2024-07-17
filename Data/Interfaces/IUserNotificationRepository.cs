using Data.Entities;

namespace Data.Interfaces
{
    public interface IUserNotificationRepository
    {
        Task<IEnumerable<UserNotification>> GetAllAsync();

        Task<UserNotification> GetByIdAsync(int id);

        Task AddAsync(UserNotification entity);

        void Delete(UserNotification entity);

        Task DeleteByIdAsync(int id);

        void Update(UserNotification entity);
    }
}
