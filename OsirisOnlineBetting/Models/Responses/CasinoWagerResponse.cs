namespace OsirisOnlineBetting.Models.Responses
{
    public class CasinoWagerResponse
    {
        public IEnumerable<CasinoResponse>? Data { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public int TotalPages { get; set; }
    }

    public class CasinoResponse
    {
        public Guid WagerId { get; set; }
        public string? GameName { get; set; }
        public string? Provider { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}
