
namespace OT.Assessment.App.Utilities
{
    public class MessagePublisher : IMessagePublisher
    {
        private readonly IMessageQueueConnectionHelper _connectionHelper;
        private readonly IModel _channel;
        private static readonly string _responseQueueName = "responseCasinoQueue";
        public MessagePublisher(IMessageQueueConnectionHelper connectionHelper) //inject configuration and get queue name
        {
            _connectionHelper = connectionHelper;
            _channel = _connectionHelper.GetChannel();
        }
        public void Publish<T>(T message, string correlationId, string routingKey = "casinoqueue")
        {
            _connectionHelper.RefreshConnection();
            var properties = _channel.CreateBasicProperties();
            properties.CorrelationId = correlationId;
            properties.ReplyTo = _responseQueueName;
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
            lock (_channel)
            {
                _channel.BasicPublish(exchange: "", routingKey: routingKey, basicProperties: null, body: body);
            }

        }
    }
}
