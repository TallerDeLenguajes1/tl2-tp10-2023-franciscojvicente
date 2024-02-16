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
                var idUser = HttpContext.Session.GetInt32("Id");
                if(idUser == null) return NoContent();
                var tareas = _taskRepository.GetAllTareasByUser((int)idUser);
                var getTareasViewModel = new GetTareasViewModel(tareas);
                return View(getTareasViewModel);
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
                CreateTaskViewModel createTaskViewModel = new();
                createTaskViewModel.Usuarios = _userRepository.GetAllName();
                createTaskViewModel.IdTablero = idBoard;
                if (IsOperator())
                {
                    var idUser = HttpContext.Session.GetInt32("Id");
                    if(idUser == null) return NoContent();
                    return View(createTaskViewModel);
                }
                if (IsAdmin())
                {
                    return View(createTaskViewModel);
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
        public IActionResult? CreateTask(CreateTaskViewModel createTaskViewModel)
        {
            try
            {
                if(!IsLogged()) return RedirectToAction("Login/Index");
                var tarea = new Tarea(createTaskViewModel);
                if (tarea == null) return null;
                _taskRepository.Create(tarea);
                _logger.LogInformation($"Tarea {tarea.Nombre} creada correctamente");
                var idOwnerBoard = _boardRepository.GetById(createTaskViewModel.IdTablero).Id_usuario_propietario;
                return RedirectToAction("GetTasksInBoard", "Task", new { idBoard = createTaskViewModel.IdTablero, idOwnerBoard = idOwnerBoard });
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
                UpdateTaskViewModel updateTaskViewModel = new(tarea);
                updateTaskViewModel.Usuarios = _userRepository.GetAllName();
                if (IsOperator())
                {
                    var idUser = HttpContext.Session.GetInt32("Id");
                    if(idUser == null) return NoContent();
                    updateTaskViewModel.Tableros = _boardRepository.GetAllIDByUser((int)idUser);
                    return View(updateTaskViewModel);
                }
                if (IsAdmin())
                {
                    updateTaskViewModel.Tableros = _boardRepository.GetAllName();
                    return View(updateTaskViewModel); 
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
        public IActionResult? UpdateTask(UpdateTaskViewModel updateTaskViewModel)
        {
            try
            {
                if(!IsLogged()) return RedirectToAction("Login/Index");
                var tarea = new Tarea(updateTaskViewModel);
                _taskRepository.Update(tarea, tarea.Id);
                _logger.LogInformation($"Tarea {tarea.Nombre} modificada correctamente");
                var idOwnerBoard = _boardRepository.GetById(updateTaskViewModel.IdTablero).Id_usuario_propietario;
                return RedirectToAction("GetTasksInBoard", "Task", new { idBoard = updateTaskViewModel.IdTablero, idOwnerBoard = idOwnerBoard });
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