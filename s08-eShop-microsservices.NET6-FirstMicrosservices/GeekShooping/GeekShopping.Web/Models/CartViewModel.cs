using System.Collections.Generic;

namespace GeekShpping.Web.Models;

public class CartViewModel
{
    public CartHeaderViewModel CartHeader { get; set; }

    public IEnumerable<CartDetailViewModel> CartDetails { get; set; }
}
