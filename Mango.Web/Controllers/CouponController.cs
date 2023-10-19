﻿using Mango.Web.Models;
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
            var list = new List<CouponDto>();

            var response = await _couponService.GetAllCouponsAsync();

            if(response != null && response.IsSuccess) {
                list = JsonConvert.DeserializeObject<List<CouponDto>>(response.Result.ToString());
            }

            return View(list);
        }
    }
}
