namespace Data.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }

        IPersonRepository PersonRepository { get; }

        IRoleRepository RoleRepository { get; }

        IGradeRepository GradeRepository { get; }

        IFriendshipRepository FriendshipRepository { get; }

        IMessageRepository MessageRepository { get; }

        ILeaderboardRepository LeaderboardRepository { get; }

        IFeedbackRepository FeedbackRepository { get; }

        // Transactions
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();

        // Database save
        Task SaveAsync();
    }
}
