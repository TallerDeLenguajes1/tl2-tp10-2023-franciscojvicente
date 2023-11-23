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
            readonly UsuarioRepository repositorioUser = new();
            private static List<Usuario> usuarios = new();


            public IActionResult Index()
            {
                if(!IsLogged()) return RedirectToAction("Login/Index");
                if(!IsAdmin()) return RedirectToRoute(new { controller = "Home", action = "Index" });
                usuarios = repositorioUser.GetAll();
                return View(usuarios);
            }

            [HttpGet]
            public IActionResult CreateUser()
            {
                if(!IsLogged()) return RedirectToAction("Login/Index");
                if(!IsAdmin()) return RedirectToRoute(new { controller = "Home", action = "Index" });
                return View(new AltaUsuarioViewModel());
            }
    
            [HttpPost]
            public IActionResult CreateUser(Usuario usuario)
            {
                repositorioUser.Create(usuario);
                return RedirectToAction("Index");
            }

            [HttpGet]
            public IActionResult UpdateUser(int idUser)
            {
                if(!IsLogged()) return RedirectToAction("Login/Index");
                if(!IsAdmin()) return RedirectToRoute(new { controller = "Home", action = "Index" });
                var usuarioBuscado = usuarios.FirstOrDefault(usuario => usuario.Id == idUser);
                if (usuarioBuscado == null) return NoContent();
                var usuarioModificar = new UpdateUserViewModel(usuarioBuscado);
                return View(usuarioModificar);
            }

            [HttpPost]
            public IActionResult? UpdateUser(Usuario usuario)
            {
                repositorioUser.Update(usuario, usuario.Id);
                return RedirectToAction("Index");
            }

            public IActionResult DeleteUser(int idUser)
            {
                if(!IsLogged()) return RedirectToAction("Login/Index");
                if(!IsAdmin()) return RedirectToRoute(new { controller = "Home", action = "Index" });
                repositorioUser.Delete(idUser);
                return RedirectToAction("Index");
            }

            private bool IsLogged()
            {
            if (HttpContext.Session != null) return true;
            return false;
            }
            private bool IsAdmin()
            {
                if (HttpContext.Session.GetString("Rol") == "Administrador") return true;
                return false;
            }
        }
}