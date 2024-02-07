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
                _logger.LogError(ex.Message);
                TempData["ErrorMessage"] = ex.Message;
                TempData["StackTrace"] = ex.StackTrace;
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            try
            {
                if(!IsLogged()) return RedirectToRoute(new { controller = "Home", action = "Index" });
                if(!IsAdmin()) return RedirectToRoute(new { controller = "Home", action = "Index" });
                return View(new CreateUserViewModel());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                TempData["ErrorMessage"] = ex.Message;
                TempData["StackTrace"] = ex.StackTrace;
                return RedirectToRoute(new {controller = "Home", action = "Error"});
            }
        }

        [HttpPost]
        public IActionResult CreateUser(CreateUserViewModel createUserViewModel)
        {
            try
            {
                var usuario = new Usuario(createUserViewModel);
                _userRepository.Create(usuario);
                _logger.LogInformation($"Usuario {usuario.NombreDeUsuario} creado correctamente");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
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
                var userWanted = _userRepository.GetById(idUser);
                if (userWanted == null) return NoContent();
                var userUpdate = new UpdateUserViewModel(userWanted.NombreDeUsuario, (Roles)userWanted.Rol, userWanted.Id);
                #pragma warning disable CS8629 // Desactivo warning de null
                userUpdate.IdLogueado = (int)HttpContext.Session.GetInt32("Id");
                #pragma warning restore CS8629 // Activo warning de null
                return View(userUpdate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
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
                var usuario = new Usuario(updateUserViewModel);
                _userRepository.Update(usuario.NombreDeUsuario, (Roles)usuario.Rol, usuario.Id);
                _logger.LogInformation($"Usuario {usuario.NombreDeUsuario} modificado correctamente");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                TempData["ErrorMessage"] = ex.Message;
                TempData["StackTrace"] = ex.StackTrace;
                return RedirectToRoute(new {controller = "Home", action = "Error"});
            }
        }

        [HttpGet]
        public IActionResult UpdatePass(int idUser) {
            try
            {
                if(!IsLogged()) return RedirectToAction("Login/Index");
                if(!IsAdmin()) return RedirectToRoute(new { controller = "Home", action = "Index" });
                var userWanted = _userRepository.GetById(idUser);
                if (userWanted == null) return NoContent();
                var passUpdate = new UpdatePassViewModel(userWanted.Contrasenia, idUser);
                return View(passUpdate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                TempData["ErrorMessage"] = ex.Message;
                TempData["StackTrace"] = ex.StackTrace;
                return RedirectToRoute(new {controller = "Home", action = "Error"});
            }
        }

        [HttpPost]
        public IActionResult? UpdatePass(UpdatePassViewModel updatePassViewModel)
        {
            try
            {
                if(!IsLogged()) return RedirectToAction("Login/Index");
                if(!IsAdmin()) return RedirectToRoute(new { controller = "Home", action = "Index" });
                var user = new Usuario(updatePassViewModel);
                _userRepository.UpdatePass(user.Contrasenia, user.Id);
                _logger.LogInformation($"Usuario modificado correctamente");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
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
                _logger.LogError(ex.Message);
                TempData["ErrorMessage"] = ex.Message;
                TempData["StackTrace"] = ex.StackTrace;
                return RedirectToRoute(new {controller = "Home", action = "Error"});
            }
        }
        private bool IsLogged()
        {
            if (HttpContext.Session != null && HttpContext.Session.GetString("Id") != null && HttpContext.Session.GetString("NombreDeUsuario") != null && HttpContext.Session.GetString("Rol") != null) return true;
            throw new Exception("El usuario no se encuentra logueado");
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