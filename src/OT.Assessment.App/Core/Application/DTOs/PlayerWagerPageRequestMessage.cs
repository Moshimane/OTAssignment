namespace OT.Assessment.App.Core.Application.DTOs
{
    public class PlayerWagerPageRequestMessage : BaseMessage 
    {
        public string PlayerId { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
    }
}
