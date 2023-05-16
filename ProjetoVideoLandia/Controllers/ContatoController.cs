using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoVideoLandia.Data;
using ProjetoVideoLandia.Models;
using System.Data;

namespace ProjetoVideoLandia.Controllers
{
    public class ContatoController : Controller
    {
        private readonly VideoLandiaContext _context;
        public ContatoController(VideoLandiaContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var model = new MensagemContato();
            return View(model);
        }

        [HttpPost]
        public IActionResult Index([FromForm] MensagemContato? msg)
        {
            if (ModelState.IsValid)
            {
                _context.MensagensContato.Add(msg);
                _context.SaveChanges();
                return RedirectToAction("Confirmacao");
            }
            return View(msg);
        }

        [HttpGet]
        [Authorize(Roles = "2")]
        public IActionResult VisualizarMensagens()
        {
            var mensagens = _context.MensagensContato.ToList();
            return View(mensagens);
        }


        public IActionResult Confirmacao()
        {
            return View();
        }
    }
}
