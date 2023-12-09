using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_franciscojvicente.Models;
using tl2_tp10_2023_franciscojvicente.Repository;
using tl2_tp10_2023_franciscojvicente.ViewModel;

namespace tl2_tp10_2023_franciscojvicente.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    
    public class TareaController : Controller
    {
        private readonly ITareaRepository _tareaRepository;
        private readonly ITableroRepository _tableroRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ILogger<HomeController> _logger;

        public TareaController(ITareaRepository tareaRepository, ITableroRepository tableroRepository, IUsuarioRepository usuarioRepository, ILogger<HomeController> logger) {
            _tareaRepository = tareaRepository;
            _tableroRepository = tableroRepository;
            _usuarioRepository = usuarioRepository;
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
               if (!IsLogged()) return RedirectToAction("Login", "Index");
                if (IsOperator())
                {
                    var idUser = HttpContext.Session.GetInt32("Id");
                    if(idUser == null) return NoContent();
                    var tareas = _tareaRepository.GetAllTareasByUser((int)idUser);
                    var getTareasViewModel = new GetTareasViewModel(tareas);
                    return View(getTareasViewModel);
                }
                if (IsAdmin())
                {
                    var tareas = _tareaRepository.GetAll();
                    var getTareasViewModel = new GetTareasViewModel(tareas);
                    return View(getTareasViewModel);
                }
                return NoContent(); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return RedirectToRoute(new {controller = "Home", action = "Index"});
            }
        }

        [HttpGet]
        public IActionResult CreateTarea()
        {
            try
            {
                if(!IsLogged()) return RedirectToAction("Login/Index");
                // if(!IsAdmin()) return RedirectToRoute(new { controller = "Home", action = "Index" });
                AltaTareaViewModel altaTareaViewModel = new();
                altaTareaViewModel.Usuarios = _usuarioRepository.GetAllID();
                altaTareaViewModel.Tableros = _tableroRepository.GetAllID();
                return View(altaTareaViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return RedirectToRoute(new {controller = "Home", action = "Index"});
            }
        }

        [HttpPost]
        public IActionResult? CreateTarea(AltaTareaViewModel altaTareaViewModel)
        {
            try
            {
                if(!IsLogged()) return RedirectToAction("Login/Index");
                // if(!ModelState.IsValid) return RedirectToAction("Index");
                var tarea = new Tarea(altaTareaViewModel);
                if (tarea == null) return null;
                _tareaRepository.Create(tarea);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return RedirectToRoute(new {controller = "Home", action = "Index"});
            }
        }

        [HttpGet]
        public IActionResult UpdateTarea(int idTarea)
        {
            try
            {
                if(!IsLogged()) return RedirectToAction("Login/Index");
                var tarea = _tareaRepository.GetById(idTarea);
                if (tarea == null) return NoContent();
                UpdateTareaViewModel updateTareaViewModel = new(tarea);
                updateTareaViewModel.Usuarios = _usuarioRepository.GetAllID();
                updateTareaViewModel.Tableros = _tableroRepository.GetAllID();
                return View(updateTareaViewModel);  
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return RedirectToRoute(new {controller = "Home", action = "Index"});
            }
        }

        [HttpPost]
        public IActionResult? UpdateTarea(UpdateTareaViewModel updateTareaViewModel)
        {
            try
            {
                if(!IsLogged()) return RedirectToAction("Login/Index");
                // if(!ModelState.IsValid) return RedirectToAction("Index");
                var tarea = new Tarea(updateTareaViewModel);
                _tareaRepository.Update(tarea, tarea.Id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return RedirectToRoute(new {controller = "Home", action = "Index"});
            }
        }

        public IActionResult DeleteTarea(int idTarea)
        {
            try
            {
                if(!IsLogged()) return RedirectToAction("Login/Index");
                _tareaRepository.Delete(idTarea);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return RedirectToRoute(new {controller = "Home", action = "Index"});
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