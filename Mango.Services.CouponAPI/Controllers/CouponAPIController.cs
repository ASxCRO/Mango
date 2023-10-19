using AutoMapper;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.CouponAPI.Controllers
{
    [Route("api/CouponAPI")]
    [ApiController]
    public class CouponAPIController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public CouponAPIController(
            AppDbContext appDbContext,
            IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public ResponseDto<List<CouponDto>> Get()
        {
            try
            {
                return new ResponseDto<List<CouponDto>>
                {
                    Result = _mapper.Map<List<CouponDto>>(_appDbContext.Coupons.ToList())
                };
            }
            catch (Exception e)
            {
                return new ResponseDto<List<CouponDto>>
                {
                    IsSuccess = false,
                    Result = Enumerable.Empty<CouponDto>().ToList(),
                    Message = e.Message
                };
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public ResponseDto<CouponDto> Get(int id)
        {
            try
            {
                return new ResponseDto<CouponDto>
                {
                    Result = _mapper.Map<CouponDto>(_appDbContext.Coupons.FirstOrDefault(x => x.CouponId == id))
                };
            }
            catch (Exception e)
            {
                return new ResponseDto<CouponDto>
                {
                    IsSuccess = false,
                    Result = Enumerable.Empty<CouponDto>().FirstOrDefault(),
                    Message = e.Message
                };
            }
        }

        [HttpGet]
        [Route("GetByCode/{code}")]
        public ResponseDto<CouponDto> GetByCode(string code)
        {
            try
            {
                return new ResponseDto<CouponDto>
                {
                    Result = _mapper.Map<CouponDto>(_appDbContext.Coupons
                                    .First(x => x.CouponCode.ToLower().Equals(code.ToLower())))
                };
            }
            catch (Exception e)
            {
                return new ResponseDto<CouponDto>
                {
                    IsSuccess = false,
                    Result = Enumerable.Empty<CouponDto>().FirstOrDefault(),
                    Message = e.Message
                };
            }
        }

        [HttpPost]
        public ResponseDto<CouponDto> Post([FromBody] CouponDto couponDto)
        {
            try
            {
                var coupon = _mapper.Map<Coupon>(couponDto);
                _appDbContext.Coupons.Add(coupon);
                _appDbContext.SaveChanges();

                return new ResponseDto<CouponDto>
                {
                    Result = _mapper.Map<CouponDto>(coupon)
                };
            }
            catch (Exception e)
            {
                return new ResponseDto<CouponDto>
                {
                    IsSuccess = false,
                    Result = Enumerable.Empty<CouponDto>().FirstOrDefault(),
                    Message = e.Message
                };
            }
        }

        [HttpPut]
        public ResponseDto<CouponDto> Put([FromBody] CouponDto couponDto)
        {
            try
            {
                var coupon = _mapper.Map<Coupon>(couponDto);
                _appDbContext.Coupons.Update(coupon);
                _appDbContext.SaveChanges();

                return new ResponseDto<CouponDto>
                {
                    Result = _mapper.Map<CouponDto>(coupon)
                };
            }
            catch (Exception e)
            {
                return new ResponseDto<CouponDto>
                {
                    IsSuccess = false,
                    Result = Enumerable.Empty<CouponDto>().FirstOrDefault(),
                    Message = e.Message
                };
            }
        }

        [HttpDelete]
        public ResponseDto<object> Delete(int id)
        {
            try
            {
                var coupon = _appDbContext.Coupons.First(x=>x.CouponId == id);
                _appDbContext.Coupons.Remove(coupon);
                _appDbContext.SaveChanges();

                return new ResponseDto<object>
                {
                    Result = null,
                };
            }
            catch (Exception e)
            {
                return new ResponseDto<object>
                {
                    IsSuccess = false,
                    Result = Enumerable.Empty<CouponDto>().FirstOrDefault(),
                    Message = e.Message
                };
            }
        }
    }
}
