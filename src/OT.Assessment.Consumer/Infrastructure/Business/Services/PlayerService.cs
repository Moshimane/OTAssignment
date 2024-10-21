
namespace OT.Assessment.Consumer.Infrastructure.Business.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _playerRepository;
        public PlayerService(IPlayerRepository playerRepository)
        { 
            _playerRepository = playerRepository;
        }
        public async Task AddCasinoWagerAsync(CasinoWager wager)
        {
            if (wager != null)
            {
                await _playerRepository.AddCasinoWagerAsync(wager);
            }
        }

        public async Task<PlayerWagerPageResponseMessage> GetLatestPlayerWagersAsync(PlayerWagerPageRequestMessage requestMessage)
        {
            var responseMessage = new PlayerWagerPageResponseMessage()
            {
                CorrelationId = requestMessage.CorrelationId,
                MessageType = requestMessage.MessageType
            };

            responseMessage.PlayerWagerPage = await _playerRepository.GetPlayerWagersAsync(requestMessage.PlayerId, requestMessage.PageSize, requestMessage.Page);
            return responseMessage;
        }

        public async Task<TopSpenderResponseMessage> GetTopSpendersAsync(TopSpenderRequestMessage requestMessage)
        {
            var responseMessage = new TopSpenderResponseMessage()
            {
                CorrelationId = requestMessage.CorrelationId,
                MessageType = requestMessage.MessageType
            };
            responseMessage.TopSpenders =  await _playerRepository.GetTopSpendersAsync(requestMessage.Count);
            return responseMessage;
        }
    }
}
