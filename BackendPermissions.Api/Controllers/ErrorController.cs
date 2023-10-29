using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BackendPermissions.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorController : BackendPermissions.Api.Model.Controller
    {
        //private readonly MultiGarantias2.Model.Context context;

        //public ErrorController(MultiGarantias2.Model.Context context)
        //    : base(context)
        //{
        //    this.context = context;
        //}

        [Route("/error")]
        [HttpGet]
        public IActionResult Error()
        {
            IExceptionHandlerFeature context = HttpContext.Features.Get<IExceptionHandlerFeature>();

            //MailMandrill mailMandrill = new MailMandrill();

            try
            {
                if (context != null)
                {
                    //mailMandrill.SendMailErrorDetail(context.Error.Message, "Finfast BackEnd", "Error interno", string.Format("{0}\n{1}", context.Error.Message, context.Error.StackTrace), "Error", "Error no controlado, se capturó en el control global de errores de la aplicación, la información técnica presentada aportará detalles como el nombre del método que falló, y la línea en donde se produjo el error", this.CurrentEnterprise == null ? string.Empty : this.CurrentEnterprise.RutCliente);

                    return Problem(
                        detail: context.Error.StackTrace,
                        title: context.Error.Message);
                }
            }
            catch (Exception ex)
            {
                //mailMandrill.SendMailErrorDetail(context.Error.Message, "Finfast BackEnd", "Error interno", string.Format("{0}\n{1}", ex.Message, "Sin traza detectada"), "Error", "Error no controlado, se capturó en el control global de errores de la aplicación, la información técnica presentada aportará detalles como el nombre del método que falló, y la línea en donde se produjo el error", this.CurrentEnterprise == null ? string.Empty : this.CurrentEnterprise.RutCliente);
            }

            return null;
        }
    }
}
