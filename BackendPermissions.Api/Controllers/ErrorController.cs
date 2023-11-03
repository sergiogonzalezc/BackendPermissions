using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BackendPermissions.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorController : BackendPermissions.Api.Model.Controller
    {
        [Route("/error")]
        [HttpGet]
        public IActionResult Error()
        {
            IExceptionHandlerFeature context = HttpContext.Features.Get<IExceptionHandlerFeature>();
                        
            try
            {
                if (context != null)
                {

                    return Problem(
                        detail: context.Error.StackTrace,
                        title: context.Error.Message);
                }
            }
            catch (Exception ex)
            {
            
            }

            return null;
        }
    }
}
