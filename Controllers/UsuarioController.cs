using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_franciscojvicente.Models;
using tl2_tp10_2023_franciscojvicente.Repository;

namespace tl2_tp10_2023_franciscojvicente.Controllers
{
        using Microsoft.AspNetCore.Mvc;
    
        public class UsuarioController : Controller
        {
            readonly UsuarioRepository repositorioUser = new();
            private static List<Usuario> usuarios = new();
            readonly LoginController loginController = new();
            public IActionResult Index()
            {
                // if(!loginController.IsLogged()) return RedirectToAction("Login/Index");
                usuarios = repositorioUser.GetAll();
                return View(usuarios);
            }

            [HttpGet]
            public IActionResult CreateUser()
            {
                // if(!loginController.IsLogged()) return RedirectToAction("Login/Index");
                return View(new Usuario());
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
                // if(!loginController.IsLogged()) return RedirectToAction("Login/Index");
                return View(usuarios.FirstOrDefault(usuario => usuario.Id == idUser));
            }

            [HttpPost]
            public IActionResult? UpdateUser(Usuario usuario)
            {
                repositorioUser.Update(usuario, usuario.Id);
                return RedirectToAction("Index");
            }

            public IActionResult DeleteUser(int idUser)
            {
                repositorioUser.Delete(idUser);
                return RedirectToAction("Index");
            }
        }
}