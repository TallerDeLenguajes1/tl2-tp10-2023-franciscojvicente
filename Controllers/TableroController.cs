using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_franciscojvicente.Models;
using tl2_tp10_2023_franciscojvicente.Repository;

namespace tl2_tp10_2023_franciscojvicente.Controllers
{
    
        using Microsoft.AspNetCore.Mvc;
    
        public class TableroController : Controller
        {
            readonly TableroRepository repositorioTablero = new();
            readonly UsuarioRepository repositorioUser = new();
            private static List<Tablero> tableros = new();
            readonly LoginController loginController = new();
            
            public IActionResult Index()
            {
                // if(!loginController.IsLogged()) return RedirectToAction("Login/Index");
                // if(loginController.IsAdmin()) {
                tableros = repositorioTablero.GetAll();
                return View(tableros);
                // }
            //     if(loginController.IsOperator()) {
            //         // tableros = repositorioTablero.GetAllByUser();
            //         return View(tableros);
            //     }
            //     return NoContent();
            // }
            }

            [HttpGet]
            public IActionResult CreateTablero()
            {
                // if(!loginController.IsLogged()) return RedirectToAction("Login/Index");
                UsuarioTableroViewModel usuarioTableroViewModel = new();
                usuarioTableroViewModel.Usuarios = repositorioUser.GetAll();
                return View(usuarioTableroViewModel);
            }

            [HttpPost]
            public IActionResult? CreateTablero(Tablero tablero)
            {
                if (tablero == null) return null;
                repositorioTablero.Create(tablero);
                return RedirectToAction("Index");
            }

            [HttpGet]
            public IActionResult UpdateTablero(int idTablero)
            {
            // if(!loginController.IsLogged()) return RedirectToAction("Login/Index");
            UsuarioTableroViewModel usuarioTableroViewModel = new();
            usuarioTableroViewModel.Usuarios = repositorioUser.GetAll();
            usuarioTableroViewModel.Tablero = tableros.FirstOrDefault(tablero => tablero.Id == idTablero);
            return View(usuarioTableroViewModel);
            }

            [HttpPost]
            public IActionResult? UpdateTablero(Tablero tablero)
            {
                // if(!loginController.IsLogged()) return RedirectToAction("Login/Index");
                repositorioTablero.Update(tablero, tablero.Id);
                return RedirectToAction("Index");
            }

            public IActionResult DeleteTablero(int idTablero)
            {
                repositorioTablero.Delete(idTablero);
                return RedirectToAction("Index");
            }

            [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
            public IActionResult Error()
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

        }
}
