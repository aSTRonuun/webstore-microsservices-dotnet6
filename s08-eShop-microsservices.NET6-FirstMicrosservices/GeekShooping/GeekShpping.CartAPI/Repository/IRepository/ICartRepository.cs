﻿using GeekShpping.CartAPI.Data.ValueObjects;

namespace GeekShpping.CartAPI.Repository.IRepository;

public interface ICartRepository
{
    Task<CartVO> FindCartByUserId(string userId);
    Task<CartVO> SaveOrUpdateCart(CartVO cart);
    Task<bool> RemoveFromCart(long cartDetaildsId);

    Task<bool> ApplyCoupon(string userId, string couponCode);
    Task<bool> RemoveCoupon(string userId);
    Task<bool> ClearCart(string userId);
}
