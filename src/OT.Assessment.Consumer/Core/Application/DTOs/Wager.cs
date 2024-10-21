namespace OT.Assessment.Consumer.Core.Application.DTOs
{
    public class Wager
    {
        public string WagerId { get; set; }
        public string Game {  get; set; }
        public string Provider { get; set; }
        public double Amount { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
