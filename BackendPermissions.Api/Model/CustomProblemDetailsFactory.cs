using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;

namespace BackendPermissions.Api.Model
{
    public class CustomProblemDetailsFactory : ProblemDetailsFactory
    {
        public override ProblemDetails CreateProblemDetails(HttpContext httpContext, int? statusCode = null, string title = null, string type = null, string detail = null, string instance = null)
        {
            ProblemDetails problemDetails = new ProblemDetails
            {
                Title = "Error no controlado",
                Status = 400
            };

            return problemDetails;
        }

        public override ValidationProblemDetails CreateValidationProblemDetails(HttpContext httpContext, ModelStateDictionary modelStateDictionary, int? statusCode = null, string title = null, string type = null, string detail = null, string instance = null)
        {
            ValidationProblemDetails customError = new ValidationProblemDetails
            {
                Title = "Error no controlado",
                Status = 400
            };

            return customError;
        }
    }
}
