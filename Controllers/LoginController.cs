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
        private readonly ILogger<HomeController> _logger;

        public LoginController(IUsuarioRepository usuarioRepository, ILogger<HomeController> logger) {
            _usuarioRepository = usuarioRepository;
            _logger = logger;
        }

        public IActionResult Index()
        {
            if(IsLogged()) return RedirectToRoute(new {controller = "Home", action = "Index"}); 
            return View(new LoginViewModel());
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel loginViewModel)
        {
            try
            {
                if(!ModelState.IsValid) return RedirectToAction("Index");
                var usuarioLogeado = _usuarioRepository.Login(loginViewModel.NombreDeUsuario, loginViewModel.Contrasenia);
                LoginUsuario(new LoginUsuarioViewModel(usuarioLogeado));
                _logger.LogInformation($"Acceso correcto por parte del usuario {loginViewModel.NombreDeUsuario}");
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            }
        }

        public IActionResult LogOut()
        {
            try
            {
                LogoutUsuario();
                _logger.LogInformation("Sesión cerrada correctamente");
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return RedirectToRoute(new { controller = "Home", action = "Error" });
            }
        }

        private void LoginUsuario(LoginUsuarioViewModel loginUsuarioViewModel)
        {
            if (loginUsuarioViewModel.Id == 0 || loginUsuarioViewModel.NombreDeUsuario == null || loginUsuarioViewModel.Rol == null) return;
            HttpContext.Session.SetInt32("Id", loginUsuarioViewModel.Id);
            HttpContext.Session.SetString("NombreDeUsuario", loginUsuarioViewModel.NombreDeUsuario);
            HttpContext.Session.SetString("Rol", loginUsuarioViewModel.Rol.ToString());
        }
        
        private void LogoutUsuario()
        {
            if (HttpContext.Session.GetString("Id") == null && HttpContext.Session.GetString("NombreDeUsuario") == null && HttpContext.Session.GetString("Rol") == null) throw new Exception("No existe un usuario logueado.");
            HttpContext.Session.Remove("Id");
            HttpContext.Session.Remove("NombreDeUsuario");
            HttpContext.Session.Remove("Rol");
            HttpContext.Session.Clear();
        }
        private bool IsLogged()
        {
            if (HttpContext.Session != null && HttpContext.Session.GetString("Id") != null && HttpContext.Session.GetString("NombreDeUsuario") != null && HttpContext.Session.GetString("Rol") != null) return true; 
            return false;
        }
    }
}

