using Microsoft.AspNetCore.Mvc.Filters;

namespace ProjetoVideoLandia.ActionFilter
{
    public class TokenActionFilter : IActionFilter, IOrderedFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenActionFilter(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Recupera o token JWT do cookie
            string token = _httpContextAccessor.HttpContext.Request.Cookies["jwt"];

            // Verifica se o token é válido
            if (!string.IsNullOrEmpty(token))
            {
                // Inclui o token JWT no cabeçalho de autorização da solicitação
                if (!_httpContextAccessor.HttpContext.Request.Headers.ContainsKey("Authorization"))
                {
                    _httpContextAccessor.HttpContext.Request.Headers.Add("Authorization", "Bearer " + token);
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Código a ser executado após a action ser executada
        }

        public int Order => -100;
    }
}
