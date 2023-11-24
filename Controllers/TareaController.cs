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

            public TareaController(ITareaRepository tareaRepository, ITableroRepository tableroRepository, IUsuarioRepository usuarioRepository) {
                _tareaRepository = tareaRepository;
                _tableroRepository = tableroRepository;
                _usuarioRepository = usuarioRepository;
            }

            // readonly TareaRepository _tareaRepository = new();
            // readonly UsuarioRepository _usuarioRepository = new();
            // readonly TableroRepository _tableroRepository = new();
            readonly LoginController loginController = new();
            private static GetTareasViewModel getTareasViewModel = new();
            private static List<Tarea> tareas = new();
        

            public IActionResult Index()
            {
                if (!IsLogged()) return RedirectToAction("Login", "Index");

                if (IsAdmin())
                {
                    getTareasViewModel.Tareas = _tareaRepository.GetAll();
                    // tareas = _tareaRepository.GetAll();
                    return View(getTareasViewModel);
                    // return View(tareas);
                }
                if (IsOperator())
                {
                    var idUser = HttpContext.Session.GetInt32("Id");
                    if(idUser == null) return NoContent();
                    getTareasViewModel.Tareas = _tareaRepository.GetAllTareasByUser((int)idUser);
                    // tareas = _tareaRepository.GetAllTareasByUser((int)idUser);
                    // return View(tareas);
                    return View(getTareasViewModel);
                }
                return NoContent();
            }
    
            [HttpGet]
            public IActionResult CreateTarea()
            {
                if(!IsLogged()) return RedirectToAction("Login/Index");
                if(!IsAdmin()) return RedirectToRoute(new { controller = "Home", action = "Index" });
                AltaTareaViewModel altaTareaViewModel = new();
                altaTareaViewModel.Usuarios = _usuarioRepository.GetAll();
                altaTareaViewModel.Tableros = _tableroRepository.GetAll();
                return View(altaTareaViewModel);
            }

            [HttpPost]
            public IActionResult? CreateTarea(AltaTareaViewModel altaTareaViewModel)
            {
                if(!IsLogged()) return RedirectToAction("Login/Index");
                if(!ModelState.IsValid) return RedirectToAction("Index");
                var tarea = new Tarea(altaTareaViewModel);
                if (tarea == null) return null;
                _tareaRepository.Create(tarea);
                return RedirectToAction("Index");
            }

            [HttpGet]
            public IActionResult UpdateTarea(int idTarea)
            {
                if(!IsLogged()) return RedirectToAction("Login/Index");
                UpdateTareaViewModel updateTareaViewModel = new();
                updateTareaViewModel.Usuarios = _usuarioRepository.GetAll();
                updateTareaViewModel.Tableros = _tableroRepository.GetAll();
                // if (tareas == null) return NoContent();
                if (getTareasViewModel.Tareas == null) return NoContent();
                // updateTareaViewModel.Tarea = tareas.FirstOrDefault(tarea => tarea.Id == idTarea);
                // updateTareaViewModel.Tarea = getTareasViewModel.Tareas.FirstOrDefault(tarea => tarea.Id == idTarea);
                return View(updateTareaViewModel);
            }

            [HttpPost]
            public IActionResult? UpdateTarea(UpdateTareaViewModel updateTareaViewModel)
            {
                if(!IsLogged()) return RedirectToAction("Login/Index");
                if(!ModelState.IsValid) return RedirectToAction("Index");
                var tarea = new Tarea(updateTareaViewModel);
                _tareaRepository.Update(tarea, tarea.Id);
                return RedirectToAction("Index");
            }

            public IActionResult DeleteTarea(int idTarea)
            {
                if(!IsLogged()) return RedirectToAction("Login/Index");
                _tareaRepository.Delete(idTarea);
                return RedirectToAction("Index");
            }
            private bool IsLogged()
            {
            if (HttpContext.Session != null) return true;
            return false;
            }
            private bool IsAdmin()
            {
                if (HttpContext.Session.GetString("Rol") == "Administrador") return true;
                return false;
            }
            private bool IsOperator()
            {
                if (HttpContext.Session != null && HttpContext.Session.GetString("Rol") == "Operador") return true;
                return false;
            }

        }
}