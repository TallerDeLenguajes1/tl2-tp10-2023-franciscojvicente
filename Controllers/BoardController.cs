using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_franciscojvicente.Models;
using tl2_tp10_2023_franciscojvicente.ViewModel;
using tl2_tp10_2023_franciscojvicente.Repository;

namespace tl2_tp10_2023_franciscojvicente.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    public class BoardController : Controller
    {
        private readonly IBoardRepository _boardRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly ILogger<HomeController> _logger;

        public BoardController(IBoardRepository boardRepository, IUserRepository userRepository, ITaskRepository taskRepository, ILogger<HomeController> logger) {
                _boardRepository = boardRepository;
                _userRepository = userRepository;
                _taskRepository = taskRepository;
                _logger = logger;
        }
        
        public IActionResult Index()
        {
            try
            {
                if (!IsLogged()) return RedirectToRoute(new {controller = "Login", action = "Index"});
                if (IsOperator())
                {
                    var idUser = HttpContext.Session.GetInt32("Id");
                    if(idUser == null) return NoContent();
                    var tableros = _boardRepository.GetAllOwnAndAssigned((int)idUser);
                    var getTablerosViewModel = new GetTablerosViewModel(tableros);
                    return View(getTablerosViewModel);
                }
                if (IsAdmin())
                {
                    var tableros = _boardRepository.GetAll();
                    var getTablerosViewModel = new GetTablerosViewModel(tableros);
                    return View(getTablerosViewModel);
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                TempData["ErrorMessage"] = ex.Message;
                TempData["StackTrace"] = ex.StackTrace;
                return RedirectToRoute(new {controller = "Home", action = "Index"});
            }
        }

        [HttpGet]
        public IActionResult CreateBoard()
        {
            try
            {
                if(!IsLogged()) return RedirectToRoute(new {controller = "Home", action = "Index"});
                CreateBoardViewModel createBoardViewModel = new();
                createBoardViewModel.Usuarios = _userRepository.GetAllID();
                return View(createBoardViewModel);
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
        public IActionResult? CreateBoard(CreateBoardViewModel createBoardViewModel)
        {
            try
            {    
                if(!IsLogged()) return RedirectToRoute(new {controller = "Home", action = "Index"});
                var tablero = new Tablero(createBoardViewModel);
                if (tablero == null) return null;
                _boardRepository.Create(tablero);
                _logger.LogInformation($"Tablero {tablero.Nombre} creado correctamente");
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
        public IActionResult UpdateBoard(int idTablero)
        {
            try
            {
                if(!IsLogged()) return RedirectToAction("Login/Index");
                var tablero = _boardRepository.GetById(idTablero);
                if (tablero == null) return NoContent();
                UpdateBoardViewModel updateBoardViewModel = new(tablero);
                updateBoardViewModel.Usuarios = _userRepository.GetAllID();
                return View(updateBoardViewModel);    
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
        public IActionResult? UpdateBoard(UpdateBoardViewModel updateBoardViewModel)
        {
            try
            {
                if(!IsLogged()) return RedirectToAction("Login/Index");
                var tablero = new Tablero(updateBoardViewModel);
                _boardRepository.Update(tablero, tablero.Id);
                _logger.LogInformation($"Tablero {tablero.Nombre} modificado correctamente");
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

        public IActionResult DeleteBoard(int idTablero)
        {
            try
            {
                if(!IsLogged()) return RedirectToAction("Login/Index");
                _taskRepository.DeleteByBoard(idTablero);
                _logger.LogInformation($"Se eliminaron las tareas correspondientes al tablero {idTablero}");
                _boardRepository.Delete(idTablero);
                _logger.LogInformation($"Tablero {idTablero} eliminado correctamente");
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
            throw new Exception ($"El usuario no se encuentra logueado.");
        }
        private bool IsAdmin()
        {
            if (HttpContext.Session.GetString("Rol") == "Administrador") return true;
            throw new Exception ($"El usuario {HttpContext.Session.GetString("NombreDeUsuario")} no cuenta con los permisos de administrador necesarios.");
        }
        
        private bool IsOperator()
        {
            if (HttpContext.Session.GetString("Rol") == "Operador") return true;
            return false;
        }
    }
}
