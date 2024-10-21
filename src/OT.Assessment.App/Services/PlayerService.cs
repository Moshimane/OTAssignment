namespace OT.Assessment.App.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IMessagePublisher _messagePublisher;
        private readonly IMessageSubscriber _messageSubscriber;
        public PlayerService(IMessagePublisher messagePublisher, IMessageSubscriber messageSubscriber)
        {
            _messagePublisher = messagePublisher;
            _messageSubscriber = messageSubscriber;
        }

        public void AddCasinoWager(CasinoWager wager)
        {
            var request = new AddCasinoWagerMessage()
            {
                CasinoWager = wager,
                CorrelationId = Guid.NewGuid().ToString(),
                MessageType = "AddCasinoWager"
            };

            _messagePublisher.Publish(request, request.CorrelationId, "requestCasinoQueue");
        }

        public async Task<List<TopSpender>> TopSpendersAsync(int count)
        {
            var request = new TopSpenderRequestMessage()
            {
                Count = count,
                MessageType = "GetTopSpenders",
                CorrelationId = Guid.NewGuid().ToString()
            };

            _messagePublisher.Publish(request, request.CorrelationId, "requestCasinoQueue");

            var response = await _messageSubscriber.SubscribeAsync<TopSpenderResponseMessage>("responseCasinoQueue", request.CorrelationId);

            return response.TopSpenders;
        }

        public async Task<PlayerWagerPage> GetLatestPlayerWagersAsync(string playerId, int pageSize, int page)
        {
            var request = new PlayerWagerPageRequestMessage()
            {
                PlayerId = playerId,
                PageSize = pageSize,
                Page = page,
                MessageType = "GetLatestCasinoWagers",
                CorrelationId = Guid.NewGuid().ToString()
            };

            _messagePublisher.Publish(request, request.CorrelationId, "requestCasinoQueue");

            var response = await _messageSubscriber.SubscribeAsync<PlayerWagerPageResponseMessage>("responseCasinoQueue", request.CorrelationId);

            return response.PlayerWagerPage;
        }
    }
}
