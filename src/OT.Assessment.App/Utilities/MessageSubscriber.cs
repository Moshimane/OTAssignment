
namespace OT.Assessment.App.Utilities
{
    public class MessageSubscriber : IMessageSubscriber
    {
        private readonly IMessageQueueConnectionHelper _connectionHelper;
        public MessageSubscriber(IMessageQueueConnectionHelper connectionHelper)
        {
            _connectionHelper = connectionHelper;
        }
        public async Task<T> SubscribeAsync<T>(string routeKey, string correlationId)
        {
            var channel = _connectionHelper.GetChannel();
            var taskCompletionSource = new TaskCompletionSource<T>();

            channel.BasicQos(0, 10, false);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                if (ea.BasicProperties.CorrelationId == correlationId)
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var baseMessage = JsonConvert.DeserializeObject<T>(message);
                    taskCompletionSource.SetResult(baseMessage);
                    channel.BasicAck(ea.DeliveryTag, false);
                    await Task.Yield();
                }
            };

            channel.BasicConsume(queue: routeKey, autoAck: true, consumer: consumer);

            return await taskCompletionSource.Task;
        }
    }
}
