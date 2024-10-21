
namespace OT.Assessment.Consumer.Core.Application.DTOs
{
    public class ResponseMessage<T> where T : class
    {
        public T Content { get; set; }
    }
}
