
namespace OT.Assessment.Consumer.Infrastructure.Business.Services
{
    public class MessageSubscriberService : BackgroundService
    {
        private readonly IMessageQueueConnectionHelper _connectionHelper;
        private readonly IPlayerService _playerService;
        private readonly IModel _channel;

        public MessageSubscriberService(IMessageQueueConnectionHelper connectionHelper, IPlayerService playerService) 
        { 
            _connectionHelper = connectionHelper;
            _playerService = playerService;
            _channel = connectionHelper.GetChannel();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(message);
                var baseMessage = JsonConvert.DeserializeObject<BaseMessage>(message);

                switch(baseMessage.MessageType)
                {
                    case "AddCasinoWager":
                        var addCasinoWagerMessage = JsonConvert.DeserializeObject<AddCasinoWagerMessage>(message);
                        await _playerService.AddCasinoWagerAsync(addCasinoWagerMessage.CasinoWager);
                        break;
                    case "GetTopSpenders":
                        var topSpenderRequestMessage = JsonConvert.DeserializeObject<TopSpenderRequestMessage>(message);
                        var topSpenderResponse = await _playerService.GetTopSpendersAsync(topSpenderRequestMessage);
                        Publish(topSpenderResponse, topSpenderResponse.CorrelationId);
                        break;
                    case "GetLatestCasinoWagers":
                        var playerWagerPageRequestMessage = JsonConvert.DeserializeObject<PlayerWagerPageRequestMessage>(message);
                        var playerWagerPageResponse = await _playerService.GetLatestPlayerWagersAsync(playerWagerPageRequestMessage);
                        Publish(playerWagerPageResponse, playerWagerPageResponse.CorrelationId);
                        break;
                }
            };

            _channel.BasicConsume(queue: "requestCasinoQueue", autoAck: true, consumer: consumer);
            await Task.CompletedTask;
        }

        public override void Dispose()
        {
            _connectionHelper.Dispose();
            base.Dispose();
        }

        private void Publish<T>(T message, string correlationId, string routingKey = "responseCasinoQueue")
        {
            var properties = _channel.CreateBasicProperties();
            properties.CorrelationId = correlationId;

            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
            _channel.BasicPublish(exchange: "", routingKey: routingKey, basicProperties: properties, body: body);
        }
    }
}
