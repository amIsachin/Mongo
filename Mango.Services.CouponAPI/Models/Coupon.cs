﻿using System.ComponentModel.DataAnnotations;

namespace Mango.Services.CouponAPI.Models;

public class Coupon
{
    [Key]
    public int CouponId { get; set; }

    public string CouponCode { get; set; }

    public double CouponAmount { get; set; }

    public int MinAmount { get; set; }
}
