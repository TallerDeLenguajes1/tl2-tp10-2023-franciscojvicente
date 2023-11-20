using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_franciscojvicente.Models;
using tl2_tp10_2023_franciscojvicente.Repository;

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
                // if(!loginController.IsLogged()) return RedirectToAction("Login/Index");
                tareas = repositorioTarea.GetAll();
                return View(tareas);
            }
    
            [HttpGet]
            public IActionResult CreateTarea()
            {
                // if(!loginController.IsLogged()) return RedirectToAction("Login/Index");
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
                // if(!loginController.IsLogged()) return RedirectToAction("Login/Index");
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
                repositorioTarea.Delete(idTarea);
                return RedirectToAction("Index");
            }

        }
}