namespace OT.Assessment.App.Utilities.Helpers
{
    public class MessageQueueConnectionHelper : IMessageQueueConnectionHelper, IDisposable
    {
        private static IMessageQueueConnectionHelper _instance = null;
        private static readonly object _instanceLock = new object();
        private static ConnectionFactory _connectionFactory;
        private static IModel _channel;
        private static IConnection _connection;

        public static IMessageQueueConnectionHelper GetInstance(IServiceProvider serviceProvider)
        {
            if (_instance == null)
            {
                lock (_instanceLock)
                    _instance = new MessageQueueConnectionHelper();
            }
            return _instance;
        }
        public IModel GetChannel()
        {
            RefreshConnection();
            return _channel;
        }

        public void RefreshConnection()
        {
            if (_connection == null || !_connection.IsOpen)
            {
                CreateConnection();
            }
            else
            {
                if (_channel == null || !_channel.IsOpen)
                {
                    CreateChannel();
                }
            }
        }

        private MessageQueueConnectionHelper()
        {
            CreateConnection();
        }

        private void CreateConnection()
        {
            try
            {
                _connectionFactory = new ConnectionFactory()
                {
                    HostName = "localhost",
                    UserName = "guest",
                    Password = "guest",
                    Port = 5672,
                    RequestedConnectionTimeout = TimeSpan.FromMinutes(5),
                    SocketReadTimeout = TimeSpan.FromMinutes(5)
                };
                _connectionFactory.ConsumerDispatchConcurrency = 5;
                _connectionFactory.ContinuationTimeout = TimeSpan.FromMinutes(10);
                _connectionFactory.HandshakeContinuationTimeout = TimeSpan.FromMinutes(10);
                _connectionFactory.RequestedConnectionTimeout = TimeSpan.FromMinutes(10);
                _connectionFactory.SocketReadTimeout = TimeSpan.FromMinutes(10);
                _connectionFactory.SocketWriteTimeout = TimeSpan.FromMinutes(10);
                _connectionFactory.RequestedHeartbeat = TimeSpan.FromMinutes(5);
                var connection = _connectionFactory.CreateConnection();
                _connection = connection;
                CreateChannel();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        private void CreateChannel()
        {
            var channel = _connection.CreateModel();
            channel.QueueDeclare(queue: "requestCasinoQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            channel.QueueDeclare(queue: "responseCasinoQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            _channel = channel;
            _channel.ContinuationTimeout = TimeSpan.FromMinutes(10);
        }

        public void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
        }
    }
}
