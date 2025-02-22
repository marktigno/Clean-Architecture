using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ApiController : ControllerBase
    {
        private ISender? _sender;
        private IMapper? _mapper;

        protected ISender? Sender => _sender ??= HttpContext.RequestServices.GetService<ISender>();
        protected IMapper? Mapper => _mapper ??= HttpContext.RequestServices.GetService<IMapper>();
    }
}
