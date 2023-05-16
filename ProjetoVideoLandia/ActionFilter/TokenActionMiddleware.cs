using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ProjetoVideoLandia.ActionFilter
{
    public class TokenActionMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenActionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, TokenActionFilter tokenActionFilter)
        {
            var actionExecutingContext = new ActionExecutingContext(
                new ActionContext(context, context.GetRouteData(), new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor()),
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                controller: null);

            tokenActionFilter.OnActionExecuting(actionExecutingContext);
            await _next(context);
        }
    }
}
