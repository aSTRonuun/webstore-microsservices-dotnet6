﻿using GeekShopping.CartAPI.Data.ValueObjects;
using GeekShpping.CartAPI.Data.ValueObjects;
using GeekShpping.CartAPI.Messages;
using GeekShpping.CartAPI.RabbitMQSender;
using GeekShpping.CartAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GeekShpping.CartAPI.Controller;

[Route("api/v1/[controller]")]
[ApiController]
public class CartController : ControllerBase
{
    private ICartRepository _cartRepository;
    private ICouponRepository _couponRepository;
    private IRabbitMQMessageSender _rabbitMQMessageSender;

    public CartController(ICartRepository cartrepository, 
        ICouponRepository couponpository, 
        IRabbitMQMessageSender rabbitMQMessageSender)
    {
        _cartRepository = cartrepository ?? throw new ArgumentNullException(nameof(cartrepository));
        _couponRepository = couponpository ?? throw new ArgumentNullException(nameof(couponpository));
        _rabbitMQMessageSender = rabbitMQMessageSender ?? throw new ArgumentNullException(nameof(rabbitMQMessageSender));
    }

    [HttpGet("find-cart/{id}")]
    public async Task<ActionResult<CartVO>> FindById(string id)
    {
        var cart = await _cartRepository.FindCartByUserId(id);
        if (cart == null) return NotFound();
        return Ok(cart);
    }
    
    [HttpPost("add-cart")]
    public async Task<ActionResult<CartVO>> AddCart(CartVO vo)
    {
        var cart = await _cartRepository.SaveOrUpdateCart(vo);
        if (cart == null) return BadRequest();
        return Ok(cart);
    }
    
    [HttpPut("update-cart")]
    public async Task<ActionResult<CartVO>> UpdateCart(CartVO vo)
    {
        var cart = await _cartRepository.SaveOrUpdateCart(vo);
        if (cart == null) return NotFound();
        return Ok(cart);
    }
    
    [HttpDelete("remove-cart/{id}")]
    public async Task<ActionResult<CartVO>> RemoveCart(int id)
    {
        var status = await _cartRepository.RemoveFromCart(id);
        if (!status) return NotFound();
        return Ok(status);
    }

    [HttpPost("apply-coupon")]
    public async Task<ActionResult<CartVO>> ApplyCoupon(CartVO vo)
    {
        var status = await _cartRepository.ApplyCoupon(vo.CartHeader.UserId, vo.CartHeader.CouponCode);
        if (!status) return NotFound();
        return Ok(status);
    }

    [HttpDelete("remove-coupon/{userId}")]
    public async Task<ActionResult<CartVO>> RemoveCoupon(string userId)
    {
        var status = await _cartRepository.RemoveCoupon(userId);
        if (!status) return NotFound();
        return Ok(status);
    }

    [HttpPost("checkout")]
    public async Task<ActionResult<CheckoutHeaderVO>> Checkout(CheckoutHeaderVO vo)
    {
        string token = Request.Headers["Authorization"];

        if (vo?.UserId == null) return BadRequest();
        var cart = await _cartRepository.FindCartByUserId(vo.UserId);
        if (cart == null) return NotFound();

        if (!string.IsNullOrEmpty(vo.CouponCode))
        {
            CouponVO coupon = await _couponRepository.GetCoupon(vo.CouponCode, token);
            if (vo.DiscountAmount != coupon.DiscountAmount) 
            {
                vo.DiscountAmount = coupon.DiscountAmount;
                return StatusCode(412);
            }
        }
        
        vo.CartDetails = cart.CartDetails;
        vo.DateTime = DateTime.Now;

        // Task RabbitMQ logic comes here!!
        _rabbitMQMessageSender.SendMessage(vo, "checkoutqueue");

        return Ok(vo);
    }
}
