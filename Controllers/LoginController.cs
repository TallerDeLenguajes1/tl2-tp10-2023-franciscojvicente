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
            private static List<Usuario> usuarios = new ();
            readonly UsuarioRepository repositorioUser = new();
            
            public IActionResult Index()
            {
                return View();
            }

            [HttpPost]
            public IActionResult Login(Usuario usuario)
            {
                usuarios = repositorioUser.GetAll();
                //existe el usuario?
                var usuarioLogeado = usuarios.FirstOrDefault(u => u.NombreDeUsuario == usuario.NombreDeUsuario && u.Contrasenia == usuario.Contrasenia);

                // si el usuario no existe devuelvo al index
                if (usuarioLogeado == null) return RedirectToAction("Index");
                
                //Registro el usuario
                LoguearUsuario(usuarioLogeado);
                
                //Devuelvo el usuario al Home
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            private void LoguearUsuario(Usuario usuario)
            {
                if (usuario.NombreDeUsuario == null || usuario.Contrasenia == null || usuario.Rol == null) return;
                HttpContext.Session.SetString("NombreDeUsuario", usuario.NombreDeUsuario);
                HttpContext.Session.SetString("Contrasenia", usuario.Contrasenia);
                HttpContext.Session.SetString("Rol", usuario.Rol.ToString());
            }

            public bool IsLogged()
            {
            if (HttpContext.Session != null) return true;
            return false;
            }

            public bool IsAdmin()
            {
                if (HttpContext.Session != null && HttpContext.Session.GetString("rol") == "Administrador") return true;
                return false;
            }

            public bool IsOperator()
            {
                if (HttpContext.Session != null && HttpContext.Session.GetString("rol") == "Operador") return true;
                return false;
            }
    }
}

