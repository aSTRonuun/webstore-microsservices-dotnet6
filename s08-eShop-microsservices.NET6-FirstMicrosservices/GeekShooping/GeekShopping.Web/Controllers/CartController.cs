using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.Web.Controllers;

public class CartController : Controller
{
    public IActionResult CartIndex()
    {
        return View();
    }
}
