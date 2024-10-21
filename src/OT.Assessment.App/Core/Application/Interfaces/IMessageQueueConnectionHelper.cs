

namespace OT.Assessment.App.Core.Application.Interfaces
{
    public interface IMessageQueueConnectionHelper
    {
        IModel GetChannel();
        void RefreshConnection();
    }
}
