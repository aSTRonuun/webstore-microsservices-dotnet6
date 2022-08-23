using GeekShpping.OrderAPI.Model;

namespace GeekShopping.OderAPI.Repository.IRepository;

public interface IOrderRepository
{
    Task<bool> AddOrder(OrderHeader header);
    Task UpdateOrderPaymentStatus(long orderHeaderId, bool paid);
}
