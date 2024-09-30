namespace OsirisOnlineBetting.Models.Requests
{
    public class PlayerWagersRequest
    {
        public Guid PlayerId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
