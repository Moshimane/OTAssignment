
namespace OT.Assessment.Consumer.Core.Application.Interfaces
{
    public interface IPlayerRepository
    {
        Task<List<TopSpender>> GetTopSpendersAsync(int count);
        Task<PlayerWagerPage> GetPlayerWagersAsync(string playerId, int pageSize, int page);
        Task AddCasinoWagerAsync(CasinoWager wager);
    }
}
