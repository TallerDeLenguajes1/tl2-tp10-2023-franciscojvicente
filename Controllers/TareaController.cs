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
            private static List<Tarea> tareas = new();
        

            public IActionResult Index()
            {
                if (!IsLogged()) return RedirectToAction("Login", "Index");

                if (IsAdmin())
                {
                    tareas = repositorioTarea.GetAll();
                    return View(tareas);
                }
                if (IsOperator())
                {
                    // Código para operadores
                    // tareas = repositorioTarea.GetAllByUser();
                    return View(tareas);
                }
                return NoContent();
            }
    
            [HttpGet]
            public IActionResult CreateTarea()
            {
                if(!IsLogged()) return RedirectToAction("Login/Index");
                if(!IsAdmin()) return RedirectToRoute(new { controller = "Home", action = "Index" });
                UserTableroTareaViewModel userTableroTareaViewModel = new();
                userTableroTareaViewModel.Usuarios = repositorioUser.GetAll();
                userTableroTareaViewModel.Tableros = repositorioTablero.GetAll();
                return View(userTableroTareaViewModel);
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
                if(!IsAdmin()) return RedirectToRoute(new { controller = "Home", action = "Index" });
                UserTableroTareaViewModel userTableroTareaViewModel = new();
                userTableroTareaViewModel.Usuarios = repositorioUser.GetAll();
                userTableroTareaViewModel.Tableros = repositorioTablero.GetAll();
                userTableroTareaViewModel.Tarea = tareas.FirstOrDefault(tarea => tarea.Id == idTarea);
                return View(userTableroTareaViewModel);
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
                if(!IsAdmin()) return RedirectToRoute(new { controller = "Home", action = "Index" });
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
                if (HttpContext.Session != null && HttpContext.Session.GetString("rol") == "Operador") return true;
                return false;
            }

        }
}