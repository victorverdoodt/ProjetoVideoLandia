using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProjetoVideoLandia.Data;
using ProjetoVideoLandia.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProjetoVideoLandia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : Controller
    {

        private readonly VideoLandiaContext _context;

        public TokenController(VideoLandiaContext context)
        {
            _context = context;
        }
        private string? ValidarCredenciais(string username, string password)
        {
            var usuario = _context.Usuarios.FirstOrDefault(x => x.Username == username && x.Password == password);
            if (usuario != null)
            {
                return usuario.Role ?? "1";
            }

            return null;
        }

        private string GerarToken(string username, string role = "1")
        {
            var claims = new[] {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("fedaf7d8863b48e197b9287d492b708e"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [AllowAnonymous]
        [HttpGet("Login")]
        public IActionResult Login()
        {
            var model = new Usuario();
            return View(model);
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login([FromForm] Usuario model)
        {
            if (ModelState.IsValid)
            {
                var role = ValidarCredenciais(model.Username, model.Password);
                if (role != null)
                {
                    var token = GerarToken(model.Username, role);
                    Response.Headers.Add("Authorization", $"Bearer {token}");
                    var cookieOptions = new CookieOptions
                    {
                        Expires = DateTime.UtcNow.AddDays(7),
                        HttpOnly = true,
                        SameSite = SameSiteMode.Strict,
                        Secure = true
                    };

                    Response.Cookies.Append("jwt", token, cookieOptions);

                    return RedirectToAction("Index", "Filmes");
                }
                else
                {
                    ModelState.AddModelError("", "Nome de usuário ou senha incorretos.");
                }
            }
            model.Password = null;
            return View(model);
        }

        [HttpGet("Logout")]
        public IActionResult Logout()
        {
            // Remova o token de autenticação do cookie ou do armazenamento local
            // Aqui, estamos removendo o token do cookie:
            Response.Cookies.Delete("Authorization");
            Response.Cookies.Delete("jwt");
            // Redirecione o usuário para a página de login
            return RedirectToAction("Index", "Filmes");
        }
    }
}
