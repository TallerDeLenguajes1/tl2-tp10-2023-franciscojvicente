using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_franciscojvicente.Models;
using tl2_tp10_2023_franciscojvicente.Repository;
using tl2_tp10_2023_franciscojvicente.ViewModel;

namespace tl2_tp10_2023_franciscojvicente.Controllers
{
    using System.Net;
    using Microsoft.AspNetCore.Mvc;
    
    public class TaskController : Controller
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IBoardRepository _boardRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<HomeController> _logger;

        public TaskController(ITaskRepository taskRepository, IBoardRepository boardRepository, IUserRepository userRepository, ILogger<HomeController> logger) {
            _taskRepository = taskRepository;
            _boardRepository = boardRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
               if (!IsLogged()) return RedirectToAction("Login", "Index");
                // if (IsOperator())
                // {
                    var idUser = HttpContext.Session.GetInt32("Id");
                    if(idUser == null) return NoContent();
                    var tareas = _taskRepository.GetAllTareasByUser((int)idUser);
                    var getTareasViewModel = new GetTareasViewModel(tareas);
                    return View(getTareasViewModel);
                // }
                // if (IsAdmin())
                // {
                //     var tareas = _taskRepository.GetAll();
                //     var getTareasViewModel = new GetTareasViewModel(tareas);
                //     return View(getTareasViewModel);
                // }
                // return NoContent(); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                TempData["ErrorMessage"] = ex.Message;
                TempData["StackTrace"] = ex.StackTrace;
                return RedirectToRoute(new {controller = "Home", action = "Index"});
            }
        }

        public IActionResult GetTasksInBoard(int idBoard, int idOwnerBoard)
        {
            try
            {
                if (!IsLogged()) return RedirectToRoute(new {controller = "Login", action = "Index"});
                var tasks = _taskRepository.GetAllTareasByTablero(idBoard);
                var getTasks = new GetTasksInBoardViewModel(tasks);
                getTasks.IDBoard = idBoard;
                getTasks.IdOwnerBoard = idOwnerBoard;
                return View(getTasks);
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
        public IActionResult CreateTask(int idBoard)
        {
            try
            {
                if(!IsLogged()) return RedirectToAction("Login/Index");
                AltaTareaViewModel altaTareaViewModel = new();
                altaTareaViewModel.Usuarios = _userRepository.GetAllID();
                altaTareaViewModel.IdTablero = idBoard;
                if (IsOperator())
                {
                    var idUser = HttpContext.Session.GetInt32("Id");
                    if(idUser == null) return NoContent();
                    // altaTareaViewModel.Tableros = _boardRepository.GetAllIDByUser((int)idUser);
                    return View(altaTareaViewModel);
                }
                if (IsAdmin())
                {
                    // altaTareaViewModel.Tableros = _boardRepository.GetAllID();
                    return View(altaTareaViewModel);
                }
                return NoContent(); 
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
        public IActionResult? CreateTask(AltaTareaViewModel altaTareaViewModel)
        {
            try
            {
                if(!IsLogged()) return RedirectToAction("Login/Index");
                // if(!ModelState.IsValid) return RedirectToAction("Index");
                var tarea = new Tarea(altaTareaViewModel);
                if (tarea == null) return null;
                _taskRepository.Create(tarea);
                _logger.LogInformation($"Tarea {tarea.Nombre} creada correctamente");
                var idOwnerBoard = _boardRepository.GetById(altaTareaViewModel.IdTablero).Id_usuario_propietario;
                return RedirectToAction("GetTasksInBoard", "Task", new { idBoard = altaTareaViewModel.IdTablero, idOwnerBoard = idOwnerBoard });
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
        public IActionResult UpdateTask(int idTarea)
        {
            try
            {
                
                
                if(!IsLogged()) return RedirectToAction("Login/Index");
                var tarea = _taskRepository.GetById(idTarea);
                if (tarea == null) return NoContent();
                UpdateTareaViewModel updateTareaViewModel = new(tarea);
                updateTareaViewModel.Usuarios = _userRepository.GetAllID();
                if (IsOperator())
                {
                    var idUser = HttpContext.Session.GetInt32("Id");
                    if(idUser == null) return NoContent();
                    updateTareaViewModel.Tableros = _boardRepository.GetAllIDByUser((int)idUser);
                    return View(updateTareaViewModel);
                }
                if (IsAdmin())
                {
                    updateTareaViewModel.Tableros = _boardRepository.GetAllID();
                    return View(updateTareaViewModel); 
                }
                return NoContent(); 
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
        public IActionResult? UpdateTask(UpdateTareaViewModel updateTareaViewModel)
        {
            try
            {
                if(!IsLogged()) return RedirectToAction("Login/Index");
                // if(!ModelState.IsValid) return RedirectToAction("Index");
                var tarea = new Tarea(updateTareaViewModel);
                _taskRepository.Update(tarea, tarea.Id);
                _logger.LogInformation($"Tarea {tarea.Nombre} modificada correctamente");
                var idOwnerBoard = _boardRepository.GetById(updateTareaViewModel.IdTablero).Id_usuario_propietario;
                return RedirectToAction("GetTasksInBoard", "Task", new { idBoard = updateTareaViewModel.IdTablero, idOwnerBoard = idOwnerBoard });
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
        public IActionResult UpdateStatusTask(int idTask, int idBoard)
        {
            try
            {
                if(!IsLogged()) return RedirectToAction("Login/Index");
                var task = _taskRepository.GetById(idTask);
                UpdateStatusTaskViewModel updateStatus = new(task.Id, task.EstadoTarea, idBoard);
                return View(updateStatus);                
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
        public IActionResult? UpdateStatusTask(UpdateStatusTaskViewModel updateStatusTaskViewModel)
        {
            try
            {
                if(!IsLogged()) return RedirectToAction("Login/Index");
                // if(!ModelState.IsValid) return RedirectToAction("Index");
                var task = _taskRepository.GetById(updateStatusTaskViewModel.Id);
                _taskRepository.UpdateStatus(updateStatusTaskViewModel.Id, (int)updateStatusTaskViewModel.EstadoTarea);
                _logger.LogInformation($"Tarea {task.Nombre} cambiada de estado correctamente");
                var idOwnerBoard = _boardRepository.GetById(updateStatusTaskViewModel.IdBoard).Id_usuario_propietario;
                return RedirectToAction("GetTasksInBoard", "Task", new { idBoard = updateStatusTaskViewModel.IdBoard, idOwnerBoard = idOwnerBoard });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                TempData["ErrorMessage"] = ex.Message;
                TempData["StackTrace"] = ex.StackTrace;
                return RedirectToRoute(new {controller = "Home", action = "Error"});
            }
        }

        public IActionResult DeleteTask(int idTarea, int idBoard)
        {
            try
            {
                if(!IsLogged()) return RedirectToAction("Login/Index");
                _taskRepository.Delete(idTarea);
                _logger.LogInformation($"Tarea {idTarea} eliminada correctamente");
                var idOwnerBoard = _boardRepository.GetById(idBoard).Id_usuario_propietario;
                return RedirectToAction("GetTasksInBoard", "Task", new { idBoard = idBoard, idOwnerBoard = idOwnerBoard });
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
            if (HttpContext.Session != null && HttpContext.Session.GetString("Rol") == "Operador") return true;
            return false;
        }
    }
}