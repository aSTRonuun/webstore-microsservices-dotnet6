using GeekShopping.OderAPI.Model.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekShpping.OrderAPI.Model;

[Table("order_header")]
public class OrderHeader : BaseEntity
{
    [Column("user_id")]
    public string UserId { get; set; }
    
    [Column("coupon_code")]
    public string? CouponCode { get; set; }
}
