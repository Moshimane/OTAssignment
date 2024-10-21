namespace OT.Assessment.Consumer.Core.Application.DTOs
{
    public class PlayerWagerPage
    {
        public List<Wager> Data { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Total {  get; set; }
        public int TotalPages { get; set; }
    }
}
