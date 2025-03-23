using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChatSupport.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]    
    public class BaseController : Controller
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
    }
}
