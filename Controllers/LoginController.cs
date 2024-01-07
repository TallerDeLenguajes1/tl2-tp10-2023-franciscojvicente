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
        private readonly IUserRepository _userRepository;
        private readonly ILogger<HomeController> _logger;

        public LoginController(IUserRepository userRepository, ILogger<HomeController> logger) {
            _userRepository = userRepository;
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
                #pragma warning disable CS8604 // Desactivo warning de nulo
                var userLogged = _userRepository.Login(loginViewModel.NombreDeUsuario, loginViewModel.Contrasenia);
                #pragma warning restore CS8604 // Activo warning de nulo
                LoginUsuario(new LoginUsuarioViewModel(userLogged));
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
            #pragma warning disable CS8604 // Desactivo warning de nulo
            HttpContext.Session.SetString("Rol", loginUsuarioViewModel.Rol.ToString());
            #pragma warning restore CS8604 // Activo warning de nulo
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

