namespace Application.Models.Payment
{
    public class OrderInfoModel
    {
        public string? OrderId { get; set; } // Optional - will be auto-generated if not provided
        public string FullName { get; set; }
        public decimal Amount { get; set; }
        public string OrderInfo { get; set; }

        public string? ExtraData { get; set; } // Optional
    }
}