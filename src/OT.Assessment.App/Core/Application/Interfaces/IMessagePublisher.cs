
namespace OT.Assessment.App.Core.Application.Interfaces
{
    public interface IMessagePublisher
    {
        void Publish<T>(T message, string correlationId, string queueKey);
    }
}
