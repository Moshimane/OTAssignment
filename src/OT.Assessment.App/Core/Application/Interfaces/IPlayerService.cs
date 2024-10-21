
namespace OT.Assessment.App.Core.Application.Interfaces
{
    public interface IPlayerService
    {
        void AddCasinoWager(CasinoWager wager);
        Task<List<TopSpender>> TopSpendersAsync(int count);
        Task<PlayerWagerPage> GetLatestPlayerWagersAsync(string playerId, int pageSize, int page);
    }
}
