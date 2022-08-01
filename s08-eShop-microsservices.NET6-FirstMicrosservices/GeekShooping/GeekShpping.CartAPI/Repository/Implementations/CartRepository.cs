﻿using AutoMapper;
using GeekShopping.CartAPI.Model.Context;
using GeekShpping.CartAPI.Data.ValueObjects;
using GeekShpping.CartAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace GeekShpping.CartAPI.Repository.Implementations;

public class CartRepository : ICartRepository
{
    private readonly MySQLContext _context;
    private IMapper _mapper;

    public CartRepository(MySQLContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<bool> ApplyCoupon(string userId, string couponCode)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> RemoveCoupon(string userId)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> ClearCart(string userId)
    {
        var cartHeader = await _context.CartHeaders.FirstOrDefaultAsync(c => c.UserId == userId);
        if (cartHeader != null)
        {
            _context.CartDetails.RemoveRange(
                _context.CartDetails.Where(c => c.CartHeaderId == cartHeader.Id));
            _context.CartHeaders.Remove(cartHeader);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<CartVO> FindCartByUserId(string userId)
    {
        Cart cart = new()
        {
            CartHeader = await _context.CartHeaders.FirstOrDefaultAsync(c => c.UserId == userId),
        };
        cart.CartDetails = _context.CartDetails.Where(c => c.CartHeaderId == cart.CartHeader.Id)
            .Include(c => c.Product);
        return _mapper.Map<CartVO>(cart);
    }

    public async Task<bool> RemoveFromCart(long cartDetaildsId)
    {
        try
        {
            CartDetail cartDetail = await _context.CartDetails.FirstOrDefaultAsync(c => c.Id.Equals(cartDetaildsId));
            
            int total = _context.CartDetails.Where(c => c.CartHeaderId == cartDetail.CartHeader.Id).Count();

            _context.CartDetails.Remove(cartDetail);
            
            if(total == 1)
            {
                var cartHeaderToRemove = await _context.CartHeaders.FirstOrDefaultAsync(c => c.Id == cartDetail.CartHeaderId);
                _context.CartHeaders.Remove(cartHeaderToRemove);
            }
            await _context.SaveChangesAsync();
            return true;
        } catch (Exception ex)
        {
            return false;
        }
    }

    public async Task<CartVO> SaveOrUpdateCart(CartVO vo)
    {
        Cart cart = _mapper.Map<Cart>(vo);
        // Cheks is the product is already sabed is the database if it does not exist then save
        var product = await _context.Products.FirstOrDefaultAsync(
            p => p.Id == vo.CartDetails.FirstOrDefault().ProductId);

        if (product == null)
        {
            _context.Products.Add(cart.CartDetails.FirstOrDefault().Product);
            await _context.SaveChangesAsync();
        }

        // Check is CartHeader is null
        var cartHeader = await _context.CartHeaders.AsNoTracking().FirstOrDefaultAsync(
            c => c.UserId == cart.CartHeader.UserId);

        if (cartHeader == null)
        {
            // Create CartHeader and CartDetails
            _context.CartHeaders.Add(cart.CartHeader);
            await _context.SaveChangesAsync();
            cart.CartDetails.FirstOrDefault().CartHeaderId = cart.CartHeader.Id;
            cart.CartDetails.FirstOrDefault().Product = null;
            _context.CartDetails.Add(cart.CartDetails.FirstOrDefault());
        } else
        {
            // If CartHeader is not null
            // Check if CartDetails has same product
            var cartDetail = await _context.CartDetails.AsNoTracking().FirstOrDefaultAsync(
                p => p.ProductId == vo.CartDetails.FirstOrDefault().ProductId && p.CartHeaderId == cartHeader.Id);

            if (cartDetail == null)
            {
                // Create CartDetails
                cart.CartDetails.FirstOrDefault().CartHeaderId = cart.CartHeader.Id;
                cart.CartDetails.FirstOrDefault().Product = null;
                _context.CartDetails.Add(cart.CartDetails.FirstOrDefault());
                await _context.SaveChangesAsync();
            } else
            {
                // Update product count and CartDetails
                cart.CartDetails.FirstOrDefault().Product = null;
                cart.CartDetails.FirstOrDefault().Count += cartDetail.Count;
                cart.CartDetails.FirstOrDefault().Id += cartDetail.Id;
                cart.CartDetails.FirstOrDefault().CartHeaderId = cartDetail.CartHeaderId;
                _context.CartDetails.Update(cart.CartDetails.FirstOrDefault());
                await _context.SaveChangesAsync();

            }
        }
        return _mapper.Map<CartVO>(cart);
    }
}
