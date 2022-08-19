using GeekShopping.OderAPI.Model.Base;
using GeekShpping.OrderAPI.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekShpping.OderAPI.Model;

[Table("order_detail")]
public class OrderDetail : BaseEntity
{
    public long CartHeaderId { get; set; }

    [ForeignKey("CartHeaderId")]
    public virtual CartHeader? CartHeader { get; set; }

    public long ProductId { get; set; }
    [ForeignKey("ProductId")]
    public virtual Product Product { get; set; }

    [Column("count")]
    public int Count { get; set; }
}
