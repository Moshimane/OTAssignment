
namespace OT.Assessment.Consumer.Business.Helpers
{
    public class MessageQueueConnectionHelper : IMessageQueueConnectionHelper
    {
        private static IMessageQueueConnectionHelper _instance = null;
        private readonly ConnectionFactory _connectionFactory;
        private static IModel _channel;
        private static IConnection _connection;

        public static IMessageQueueConnectionHelper GetInstance(IServiceProvider serviceProvider)
        {
            if (_instance == null)
            {
                    _instance = new MessageQueueConnectionHelper();
            }
            return _instance;
        }
        public IModel GetChannel()
        {
            return _channel;
        }

        public void RefreshChannel()
        {
            if(_connection.IsOpen)
            {
                CreateChannel();
            }
            else
            {
                CreateConnection();
            }
        }

        private MessageQueueConnectionHelper()
        {
            _connectionFactory = new ConnectionFactory() { HostName = "localhost" };
            CreateConnection();
        }

        private void CreateConnection()
        {
            var connection = _connectionFactory.CreateConnection();
            _connection = connection;
            CreateChannel();
        }
        private void CreateChannel()
        {
            var channel = _connection.CreateModel();
            channel.QueueDeclare(queue: "requestCasinoQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            channel.QueueDeclare(queue: "responseCasinoQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            _channel = channel;
        }

        public void Dispose()
        {
            _channel.Dispose();
            _connection.Dispose();
        }
    }
}
