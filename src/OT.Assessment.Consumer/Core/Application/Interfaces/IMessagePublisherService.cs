
namespace OT.Assessment.Consumer.Core.Application.Interfaces
{
    public interface IMessagePublisherService
    {
        void Publish(TopSpenderResponseMessage message, string routingKey = "casinoqueue");
    }
}
