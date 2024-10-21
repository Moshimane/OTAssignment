
namespace OT.Assessment.App.Core.Application.Interfaces
{
    public interface IMessageSubscriber
    {
        Task<T> SubscribeAsync<T>(string routeKey, string correlationId);
    }
}
