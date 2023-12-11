using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_franciscojvicente.Models;
using tl2_tp10_2023_franciscojvicente.ViewModel;
using tl2_tp10_2023_franciscojvicente.Repository;

namespace tl2_tp10_2023_franciscojvicente.Controllers
{
    using System.Net;
    using Microsoft.AspNetCore.Mvc;

    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IBoardRepository _boardRepository;
        private readonly ILogger<HomeController> _logger;

        public UserController(IUserRepository userRepository, ITaskRepository taskRepository, IBoardRepository boardRepository, ILogger<HomeController> logger) {
            _userRepository = userRepository;
            _taskRepository = taskRepository;
            _boardRepository = boardRepository;
            _logger = logger;
        }

        // public UserController(IUserRepository userRepository, ILogger<HomeController> logger) {
        //     _userRepository = userRepository;
        //     _logger = logger;
        // }

        public IActionResult Index()
        {
            try
            {
                if(!IsLogged()) return RedirectToRoute(new { controller = "Home", action = "Index" });
                if(!IsAdmin()) return RedirectToRoute(new { controller = "Home", action = "Index" });
                var usuarios = _userRepository.GetAll();
                var getUsuariosViewModel = new GetUsuariosViewModel(usuarios);
                return View(getUsuariosViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                TempData["ErrorMessage"] = ex.Message;
                TempData["StackTrace"] = ex.StackTrace;
                return RedirectToRoute(new {controller = "Home", action = "Error"});
            }
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            try
            {
                if(!IsLogged()) return RedirectToRoute(new { controller = "Home", action = "Index" });
                if(!IsAdmin()) return RedirectToRoute(new { controller = "Home", action = "Index" });
                return View(new AltaUsuarioViewModel());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                TempData["ErrorMessage"] = ex.Message;
                TempData["StackTrace"] = ex.StackTrace;
                return RedirectToRoute(new {controller = "Home", action = "Error"});
            }
        }

        [HttpPost]
        public IActionResult CreateUser(AltaUsuarioViewModel altaUsuarioViewModel)
        {
            try
            {
                // if(!ModelState.IsValid) return RedirectToAction("Index");
                var usuario = new Usuario(altaUsuarioViewModel);
                _userRepository.Create(usuario);
                _logger.LogInformation($"Usuario {usuario.NombreDeUsuario} creado correctamente");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                TempData["ErrorMessage"] = ex.Message;
                TempData["StackTrace"] = ex.StackTrace;
                return RedirectToRoute(new {controller = "Home", action = "Error"});
            }
        }

        [HttpGet]
        public IActionResult UpdateUser(int idUser)
        {
            try
            {
                if(!IsLogged()) return RedirectToAction("Login/Index");
                if(!IsAdmin()) return RedirectToRoute(new { controller = "Home", action = "Index" });
                var usuarioBuscado = _userRepository.GetById(idUser);
                if (usuarioBuscado == null) return NoContent();
                var usuarioModificar = new UpdateUserViewModel(usuarioBuscado);
                usuarioModificar.IdLogueado = (int)HttpContext.Session.GetInt32("Id");
                return View(usuarioModificar);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                TempData["ErrorMessage"] = ex.Message;
                TempData["StackTrace"] = ex.StackTrace;
                return RedirectToRoute(new {controller = "Home", action = "Error"});
            }
        }

        [HttpPost]
        public IActionResult? UpdateUser(UpdateUserViewModel updateUserViewModel)
        {
            try
            {
                if(!IsLogged()) return RedirectToAction("Login/Index");
                if(!IsAdmin()) return RedirectToRoute(new { controller = "Home", action = "Index" });
                // if(!ModelState.IsValid) return RedirectToAction("Index");
                var usuario = new Usuario(updateUserViewModel);
                _userRepository.Update(usuario, usuario.Id);
                _logger.LogInformation($"Usuario {usuario.NombreDeUsuario} modificado correctamente");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                TempData["ErrorMessage"] = ex.Message;
                TempData["StackTrace"] = ex.StackTrace;
                return RedirectToRoute(new {controller = "Home", action = "Error"});
            }
        }

        public IActionResult DeleteUser(int idUser)
        {
            try
            {
                if(!IsLogged()) return RedirectToAction("Login/Index");
                if(!IsAdmin()) return RedirectToRoute(new { controller = "Home", action = "Index" });
                IsSameID(idUser);
                _taskRepository.DeleteByUser(idUser);
                _logger.LogInformation($"Tareas del usuario {idUser} eliminadas correctamente");
                _boardRepository.DeleteByUser(idUser);
                _logger.LogInformation($"Tableros del usuario {idUser} eliminados correctamente");
                _userRepository.Delete(idUser);
                _logger.LogInformation($"Usuario {idUser} eliminado correctamente");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                TempData["ErrorMessage"] = ex.Message;
                TempData["StackTrace"] = ex.StackTrace;
                return RedirectToRoute(new {controller = "Home", action = "Error"});
            }
        }
        private bool IsLogged()
        {
            if (HttpContext.Session != null && HttpContext.Session.GetString("Id") != null && HttpContext.Session.GetString("NombreDeUsuario") != null && HttpContext.Session.GetString("Rol") != null) return true; 
            throw new Exception ($"El usuario no se encuentra logueado");
        }
        private bool IsAdmin()
        {
            if (HttpContext.Session.GetString("Rol") == "Administrador") return true;
            throw new Exception ($"El usuario {HttpContext.Session.GetString("NombreDeUsuario")} no cuenta con los permisos de administrador necesarios");
        }
        private void IsSameID(int idUser) {
            var idUserLogged = HttpContext.Session.GetInt32("Id");
            if (idUserLogged == idUser) throw new Exception("No puedes borrar el usuario que se encuentra logueado");
            return;
        }
    }
}