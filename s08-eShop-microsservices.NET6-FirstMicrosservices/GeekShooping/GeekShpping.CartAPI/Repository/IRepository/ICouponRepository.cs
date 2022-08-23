using GeekShopping.CartAPI.Data.ValueObjects;

namespace GeekShpping.CartAPI.Repository.IRepository
{
    public interface ICouponRepository
    {
        Task<CouponVO> GetCouponByCouponCode(string couponCode, string token);
    }
}
