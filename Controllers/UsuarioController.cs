using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_franciscojvicente.Models;
using tl2_tp10_2023_franciscojvicente.ViewModel;
using tl2_tp10_2023_franciscojvicente.Repository;

namespace tl2_tp10_2023_franciscojvicente.Controllers
{
    using System.Net;
    using Microsoft.AspNetCore.Mvc;

    public class UsuarioController : Controller
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ILogger<HomeController> _logger;

        public UsuarioController(IUsuarioRepository usuarioRepository, ILogger<HomeController> logger) {
            _usuarioRepository = usuarioRepository;
            _logger = logger;
        }

        public IActionResult Index()
        {
            if(!IsLogged()) return RedirectToRoute(new { controller = "Home", action = "Index" });
            if(!IsAdmin()) return RedirectToRoute(new { controller = "Home", action = "Index" });
            try
            {
                var usuarios = _usuarioRepository.GetAll();
                var getUsuariosViewModel = new GetUsuariosViewModel(usuarios);
                return View(getUsuariosViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return RedirectToRoute(new {controller = "Home", action = "Error"});
            }
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            if(!IsLogged()) return RedirectToRoute(new { controller = "Home", action = "Index" });
            if(!IsAdmin()) return RedirectToRoute(new { controller = "Home", action = "Index" });
            return View(new AltaUsuarioViewModel());
        }

        [HttpPost]
        public IActionResult CreateUser(AltaUsuarioViewModel altaUsuarioViewModel)
        {
            if(!ModelState.IsValid) return RedirectToAction("Index");
            var usuario = new Usuario(altaUsuarioViewModel);
            _usuarioRepository.Create(usuario);
            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }

        [HttpGet]
        public IActionResult UpdateUser(int idUser)
        {
            if(!IsLogged()) return RedirectToAction("Login/Index");
            if(!IsAdmin()) return RedirectToRoute(new { controller = "Home", action = "Index" });
            var usuarioBuscado = _usuarioRepository.GetById(idUser);
            if (usuarioBuscado == null) return NoContent();
            var usuarioModificar = new UpdateUserViewModel(usuarioBuscado);
            usuarioModificar.IdLogueado = (int)HttpContext.Session.GetInt32("Id");
            return View(usuarioModificar);
        }

        [HttpPost]
        public IActionResult? UpdateUser(UpdateUserViewModel updateUserViewModel)
        {
            if(!IsLogged()) return RedirectToAction("Login/Index");
            if(!IsAdmin()) return RedirectToRoute(new { controller = "Home", action = "Index" });
            if(!ModelState.IsValid) return RedirectToAction("Index");
            var usuario = new Usuario(updateUserViewModel);
            _usuarioRepository.Update(usuario, usuario.Id);
            return RedirectToAction("Index");
        }

        public IActionResult DeleteUser(int idUser)
        {
            if(!IsLogged()) return RedirectToAction("Login/Index");
            if(!IsAdmin()) return RedirectToRoute(new { controller = "Home", action = "Index" });
            var idUserLogueado = HttpContext.Session.GetInt32("Id");
            if (idUserLogueado == idUser) return RedirectToRoute(new { controller = "Home", action = "Error" });
            _usuarioRepository.Delete(idUser);
            return RedirectToAction("Index");
        }

        private bool IsLogged()
        {
        if (HttpContext.Session != null && HttpContext.Session.GetString("Id") != null && HttpContext.Session.GetString("NombreDeUsuario") != null && HttpContext.Session.GetString("Rol") != null) return true;
        return false;
        }
        private bool IsAdmin()
        {
            if (HttpContext.Session.GetString("Rol") == "Administrador") return true;
            return false;
        }
    }
}