using GeekShopping.CartAPI.Model;
using GeekShopping.CartAPI.Model.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekShpping.CartAPI.Model;

[Table("cart_detail")]
public class CartDetail : BaseEntity
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
