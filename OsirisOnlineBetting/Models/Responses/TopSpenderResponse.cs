﻿namespace OsirisOnlineBetting.Models.Responses
{
    public class TopSpenderResponse
    {
        public Guid AccountId { get; set; }
        public string? Username { get; set; }
        public decimal TotalAmountSpend { get; set; }
    }
}
