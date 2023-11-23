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
            readonly TareaRepository repositorioTarea = new();
            readonly UsuarioRepository repositorioUser = new();
            readonly TableroRepository repositorioTablero = new();
            readonly LoginController loginController = new();
            readonly GetTareasViewModel getTareasViewModel = new();
            private static List<Tarea> tareas = new();
        

            public IActionResult Index()
            {
                if (!IsLogged()) return RedirectToAction("Login", "Index");

                if (IsAdmin())
                {
                    // getTareasViewModel.Tareas = repositorioTarea.GetAll();
                    tareas = repositorioTarea.GetAll();
                    // return View(getTareasViewModel);
                    return View(tareas);
                }
                if (IsOperator())
                {
                    var idUser = HttpContext.Session.GetInt32("Id");
                    if(idUser == null) return NoContent();
                    // getTareasViewModel.Tareas = repositorioTarea.GetAllTareasByUser((int)idUser);
                    tareas = repositorioTarea.GetAllTareasByUser((int)idUser);
                    return View(tareas);
                    // return View(getTareasViewModel);
                }
                return NoContent();
            }
    
            [HttpGet]
            public IActionResult CreateTarea()
            {
                if(!IsLogged()) return RedirectToAction("Login/Index");
                if(!IsAdmin()) return RedirectToRoute(new { controller = "Home", action = "Index" });
                AltaTareaViewModel altaTareaViewModel = new();
                altaTareaViewModel.Usuarios = repositorioUser.GetAll();
                altaTareaViewModel.Tableros = repositorioTablero.GetAll();
                return View(altaTareaViewModel);
            }

            [HttpPost]
            public IActionResult? CreateTarea(Tarea tarea)
            {
                if (tarea == null) return null;
                repositorioTarea.Create(tarea);
                return RedirectToAction("Index");
            }

            [HttpGet]
            public IActionResult UpdateTarea(int idTarea)
            {
                if(!IsLogged()) return RedirectToAction("Login/Index");
                // if(!IsAdmin()) return RedirectToRoute(new { controller = "Home", action = "Index" });
                UpdateTareaViewModel updateTareaViewModel = new();
                updateTareaViewModel.Usuarios = repositorioUser.GetAll();
                updateTareaViewModel.Tableros = repositorioTablero.GetAll();
                if (tareas == null) return NoContent();
                // if (getTareasViewModel.Tareas == null) return NoContent();
                updateTareaViewModel.Tarea = tareas.FirstOrDefault(tarea => tarea.Id == idTarea);
                // updateTareaViewModel.Tarea = getTareasViewModel.Tareas.FirstOrDefault(tarea => tarea.Id == idTarea);
                return View(updateTareaViewModel);
            }

            [HttpPost]
            public IActionResult? UpdateTarea(Tarea tarea)
            {
                repositorioTarea.Update(tarea, tarea.Id);
                return RedirectToAction("Index");
            }

            public IActionResult DeleteTarea(int idTarea)
            {
                if(!IsLogged()) return RedirectToAction("Login/Index");
                // if(!IsAdmin()) return RedirectToRoute(new { controller = "Home", action = "Index" });
                repositorioTarea.Delete(idTarea);
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