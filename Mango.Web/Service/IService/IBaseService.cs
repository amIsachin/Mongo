using Mango.Web.Models;

namespace Mango.Web.Service.IService;

public interface IBaseService
{
    Task<ReponseDto?> SendAsync(RequestDto requestDto);
}
