
namespace OT.Assessment.Consumer.Core.Application.Interfaces
{
    public interface ICasinoWagerRepository
    {
        Task AddCasinoWagerAsync(CasinoWager wager);
        Task<IEnumerable<CasinoWager>> GetPlayerCasinoWagers(string playerId);
    }
}
