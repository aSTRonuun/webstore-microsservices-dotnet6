namespace GeekShopping.OderAPI.Messages
{
    public class UpdatePaymentResultVO
    {
        public long OrderId { get; set; }
        public bool status { get; set; }
        public string Email { get; set; }
    }
}
