using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_franciscojvicente.Models;
using tl2_tp10_2023_franciscojvicente.ViewModel;
using tl2_tp10_2023_franciscojvicente.Repository;

namespace tl2_tp10_2023_franciscojvicente.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class UsuarioController : Controller
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioController(IUsuarioRepository usuarioRepository) {
            _usuarioRepository = usuarioRepository;
        }

        public IActionResult Index()
        {
            if(!IsLogged()) return RedirectToAction("Login/Index");
            if(!IsAdmin()) return RedirectToRoute(new { controller = "Home", action = "Index" });
            var usuarios = _usuarioRepository.GetAll();
            var getUsuariosViewModel = new GetUsuariosViewModel(usuarios);
            return View(getUsuariosViewModel);
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
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult UpdateUser(int idUser)
        {
            if(!IsLogged()) return RedirectToAction("Login/Index");
            if(!IsAdmin()) return RedirectToRoute(new { controller = "Home", action = "Index" });
            var usuarios = _usuarioRepository.GetAll();
            var usuarioBuscado = usuarios.FirstOrDefault(usuario => usuario.Id == idUser);
            if (usuarioBuscado == null) return NoContent();
            var usuarioModificar = new UpdateUserViewModel(usuarioBuscado);
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