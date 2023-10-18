using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.CouponAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponAPIController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public CouponAPIController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet]
        public List<Coupon> Get()
        {
            try
            {
                return _appDbContext.Coupons.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public Coupon Get(int id)
        {
            try
            {
                return _appDbContext.Coupons.FirstOrDefault(x=>x.CouponId == id);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
