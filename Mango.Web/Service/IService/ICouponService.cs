using Mango.Web.Models;

namespace Mango.Web.Service.IService;

public interface ICouponService
{
    Task<ReponseDto?> GetCouponAsync(string couponCode);

    Task<ReponseDto?> GetAllCouponAsync();

    Task<ReponseDto?> GetCouponByIdAsync(int id);

    Task<ReponseDto?> CreateCouponsAsync(CouponDto couponDto);

    Task<ReponseDto?> UpdateCouponsAsync(CouponDto couponDto);

    Task<ReponseDto?> DeleteCouponsAsync(int id);
}
