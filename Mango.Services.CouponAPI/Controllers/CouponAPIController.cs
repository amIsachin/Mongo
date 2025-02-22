using AutoMapper;
using Azure;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.CouponAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        private ReponseDto _reponse;
        private IMapper _mapper;

        public CouponAPIController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _reponse = new ReponseDto();
            _mapper = mapper;
        }

        [HttpGet]
        public ReponseDto Get()
        {
            try
            {
                IEnumerable<Coupon> objList = _db.Coupons.ToList();
                _reponse.Result = _mapper.Map<IEnumerable<CouponDto>>(objList);
            }
            catch (Exception ex)
            {
                _reponse.Message = ex.Message;
                _reponse.IsSuccess = false;
            }

            return _reponse;
        }

        [HttpGet]
        [Route("{id:int}")]
        public object Get(int id)
        {
            try
            {
                var coupon = _db.Coupons.FirstOrDefault(x => x.CouponId == id);

                _reponse.Result = _mapper.Map<CouponDto>(coupon);
            }
            catch (Exception ex)
            {
                _reponse.Message = ex.Message;
                _reponse.IsSuccess = false;
            }
            return _reponse;
        }

        [HttpGet]
        [Route("GetByCode/{code}")]
        public object GetByCode(string code)
        {
            try
            {
                var coupon = _db.Coupons.First(x => x.CouponCode.ToLower() == code.ToLower());

                _reponse.Result = _mapper.Map<CouponDto>(coupon);
            }
            catch (Exception ex)
            {
                _reponse.Message = ex.Message;
                _reponse.IsSuccess = false;
            }

            return _reponse;

        }

        [HttpPost]
        public ReponseDto Post([FromBody] CouponDto couponDto)
        {
            try
            {
                var obj = _mapper.Map<Coupon>(couponDto);

                _db.Coupons.Add(obj);
                _db.SaveChanges();

                _reponse.Result = _mapper.Map<CouponDto>(obj);
            }
            catch (Exception ex)
            {
                _reponse.Message = ex.Message;
                _reponse.IsSuccess = false;
            }

            return _reponse;
        }

        [HttpPut]
        public ReponseDto Put([FromBody] CouponDto couponDto)
        {
            try
            {
                var obj = _mapper.Map<Coupon>(couponDto);

                _db.Coupons.Update(obj);
                _db.SaveChanges();

                _reponse.Result = _mapper.Map<CouponDto>(obj);
            }
            catch (Exception ex)
            {
                _reponse.Message = ex.Message;
                _reponse.IsSuccess = false;
            }

            return _reponse;
        }

        [HttpDelete]
        [Route("{id:int}")]
        public ReponseDto Delete(int id)
        {
            try
            {
                var obj = _db.Coupons.FirstOrDefault(x => x.CouponId == id);

                _db.Coupons.Remove(obj);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                _reponse.Message = ex.Message;
                _reponse.IsSuccess = false;
            }

            return _reponse;
        }
    }
}
