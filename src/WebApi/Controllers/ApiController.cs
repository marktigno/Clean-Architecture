using MapsterMapper;
using Microsoft.AspNetCore.Mvc;


namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ApiController : ControllerBase
    {
        private IMapper? _mapper;

        protected IMapper? Mapper => _mapper ??= HttpContext.RequestServices.GetService<IMapper>();
    }
}
