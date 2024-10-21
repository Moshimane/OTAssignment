
namespace OT.Assessment.Consumer.Core.Application.Interfaces
{
    public interface IMessageQueueConnectionHelper
    {
        IModel GetChannel();
        void Dispose();
    }
}
