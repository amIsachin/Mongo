using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Mango.Web.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponService _couponService;

        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }

        public async Task<IActionResult> CouponIndex()
        {
            List<CouponDto>? list = new List<CouponDto>();

            ReponseDto response = await _couponService.GetAllCouponAsync();

            if (response is not null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(response.Result));
            }

            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> CouponCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CouponCreate(CouponDto couponDto)
        {
            if (ModelState.IsValid)
            {
                ReponseDto response = await _couponService.CreateCouponsAsync(couponDto);

                if (response is not null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(CouponIndex));
                }
            }

            return View(couponDto);
        }

        [HttpGet]
        public async Task<IActionResult> CouponDelete(int couponId)
        {
            ReponseDto response = await _couponService.GetCouponByIdAsync(couponId);

            if (response is not null && response.IsSuccess)
            {
                CouponDto couponDto = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(response.Result));

                return View(couponDto);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> CouponDelete(CouponDto couponDto)
        {
            ReponseDto response = await _couponService.DeleteCouponsAsync(couponDto.CouponId);

            if (response is not null && response.IsSuccess)
            {
                return RedirectToAction(nameof(CouponIndex));
            }

            return View(couponDto);
        }
    }
}