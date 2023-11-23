using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_franciscojvicente.Models;
using tl2_tp10_2023_franciscojvicente.ViewModel;
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
                if (!IsLogged()) return RedirectToAction("Login", "Index");

                if (IsAdmin())
                {
                    tableros = repositorioTablero.GetAll();
                    return View(tableros);
                }
                if (IsOperator())
                {
                    // Código para operadores
                    // tableros = repositorioTablero.GetAllByUser();
                    return View(tableros);
                }
                return NoContent();
            }


            [HttpGet]
            public IActionResult CreateTablero()
            {
                if(!IsLogged()) return RedirectToAction("Login/Index");
                if(!IsAdmin()) return RedirectToRoute(new { controller = "Home", action = "Index" });
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
            if(!IsLogged()) return RedirectToAction("Login/Index");
            if(!IsAdmin()) return RedirectToRoute(new { controller = "Home", action = "Index" });
            UsuarioTableroViewModel usuarioTableroViewModel = new();
            usuarioTableroViewModel.Usuarios = repositorioUser.GetAll();
            usuarioTableroViewModel.Tablero = tableros.FirstOrDefault(tablero => tablero.Id == idTablero);
            return View(usuarioTableroViewModel);
            }

            [HttpPost]
            public IActionResult? UpdateTablero(Tablero tablero)
            {
                if(!IsLogged()) return RedirectToAction("Login/Index");
                if(!IsAdmin()) return RedirectToRoute(new { controller = "Home", action = "Index" });
                repositorioTablero.Update(tablero, tablero.Id);
                return RedirectToAction("Index");
            }

            public IActionResult DeleteTablero(int idTablero)
            {
                if(!IsLogged()) return RedirectToAction("Login/Index");
                if(!IsAdmin()) return RedirectToRoute(new { controller = "Home", action = "Index" });
                repositorioTablero.Delete(idTablero);
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
                if (HttpContext.Session.GetString("rol") == "Operador") return true;
                return false;
            }

            [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
            public IActionResult Error()
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

        }
}
