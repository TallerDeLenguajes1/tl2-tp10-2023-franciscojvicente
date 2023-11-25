using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_franciscojvicente.Models;
using tl2_tp10_2023_franciscojvicente.Repository;

namespace tl2_tp10_2023_franciscojvicente.Controllers
{
    using System;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.VisualBasic;
    using tl2_tp10_2023_franciscojvicente.ViewModel;

    public class LoginController : Controller
        {

            private readonly IUsuarioRepository _usuarioRepository;

            public LoginController(IUsuarioRepository usuarioRepository) {
                _usuarioRepository = usuarioRepository;
            }

        private static List<Usuario> usuarios = new ();
            // readonly UsuarioRepository _usuarioRepository = new();
            
            public IActionResult Index()
            {
                return View();
            }

            [HttpPost]
            public IActionResult Login(Usuario usuario)
            {
                if(!ModelState.IsValid) return RedirectToAction("Index");
                usuarios = _usuarioRepository.GetAll();
                var usuarioLogeado = usuarios.FirstOrDefault(u => u.NombreDeUsuario == usuario.NombreDeUsuario && u.Contrasenia == usuario.Contrasenia);
                if (usuarioLogeado == null) return RedirectToAction("Index");
                LoguearUsuario(usuarioLogeado);
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            private void LoguearUsuario(Usuario usuario)
            {
                if (usuario.NombreDeUsuario == null || usuario.Contrasenia == null || usuario.Rol == null) return;
                HttpContext.Session.SetInt32("Id", usuario.Id);
                HttpContext.Session.SetString("NombreDeUsuario", usuario.NombreDeUsuario);
                // HttpContext.Session.SetString("Contrasenia", usuario.Contrasenia);
                HttpContext.Session.SetString("Rol", usuario.Rol.ToString());
            }
    }
}

