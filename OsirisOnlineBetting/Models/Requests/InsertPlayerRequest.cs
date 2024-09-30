namespace OsirisOnlineBetting.Models.Requests
{
    public class InsertPlayerRequest
    {
        public Guid AccountId { get; set; }
        public string? Username { get; set; }
    }
}