using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;

namespace Business.Services
{
    public class LeaderboardService : ILeaderboardService
    {
        private readonly IUnitOfWork _context;
        private readonly IMapper _mapper;

        public LeaderboardService(IUnitOfWork context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> AddUserPointsAsync(LeaderboardDataModel model)
        {
            try
            {
                var existingEntity = await _context.LeaderboardRepository
                    .GetByUserIdAsync(model.UserId);
                var entity = _mapper.Map<Leaderboard>(model);

                if (existingEntity != null)
                {
                    _context.LeaderboardRepository.Update(entity);
                    await _context.SaveAsync();
                }
                
                else
                {
                    await _context.LeaderboardRepository.AddAsync(entity);
                    await _context.SaveAsync();
                }

                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<LeaderboardDataModel>> GetBestUserScoresAsync(int pageSize = 1, int count = 20)
        {
            var result = await _context.LeaderboardRepository
                .GetAllWithPaginationAsync(pageSize, count);
            List<LeaderboardDataModel> datas = new List<LeaderboardDataModel>();

            foreach (var item in result)
            {
                datas.Add(_mapper.Map<LeaderboardDataModel>(item));
            }

            return datas;
        }

        public async Task<LeaderboardDataModel> GetUserScoreAsync(string userId)
        {
            var data = await _context.LeaderboardRepository.GetByUserIdAsync(userId);
            var result = _mapper.Map<LeaderboardDataModel>(data);
            
            return result;
        }
    }
}
