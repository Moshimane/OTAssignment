
namespace OT.Assessment.Consumer.Core.Application.Interfaces
{
    public interface IPlayerService
    {
        Task<PlayerWagerPageResponseMessage> GetLatestPlayerWagersAsync(PlayerWagerPageRequestMessage requestMessage);
        Task<TopSpenderResponseMessage> GetTopSpendersAsync(TopSpenderRequestMessage requestMessage);
        Task AddCasinoWagerAsync(CasinoWager wager);
    }
}
