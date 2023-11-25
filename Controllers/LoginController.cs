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

        public IActionResult Index()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel loginViewModel)
        {
            if(!ModelState.IsValid) return RedirectToAction("Index");
            var usuarios = _usuarioRepository.GetAll();
            var usuarioLogeado = usuarios.FirstOrDefault(u => u.NombreDeUsuario == loginViewModel.NombreDeUsuario && u.Contrasenia == loginViewModel.Contrasenia);
            if (usuarioLogeado == null) return RedirectToAction("Index");
            LoginUsuario(new LoginUsuarioViewModel(usuarioLogeado));
            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }

        public IActionResult LogOut()
        {
            LogoutUsuario();
            return RedirectToRoute(new { controller = "Login", action = "Index" });
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
            HttpContext.Session.Remove("Id");
            HttpContext.Session.Remove("NombreDeUsuario");
            HttpContext.Session.Remove("Rol");
            HttpContext.Session.Clear();
        }
    }
}

