using GeekShopping.CartAPI.Data.ValueObjects;

namespace GeekShpping.CartAPI.Repository.IRepository
{
    public interface ICouponRepository
    {
        Task<CouponVO> GetCoupon(string couponCode, string token);
    }
}
