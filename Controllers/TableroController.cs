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
            // readonly GetTablerosViewModel getTablerosViewModel = new();
            private static List<Tablero> tableros = new();
            
            public IActionResult Index()
            {
                if (!IsLogged()) return RedirectToAction("Login", "Index");

                if (IsAdmin())
                {
                    // getTablerosViewModel.Tableros = repositorioTablero.GetAll();
                    tableros = repositorioTablero.GetAll();
                    return View(tableros);
                }
                if (IsOperator())
                {
                    var idUser = HttpContext.Session.GetInt32("Id");
                    if(idUser == null) return NoContent();
                    // getTablerosViewModel.Tableros = repositorioTablero.GetAllByUser((int)idUser);
                    tableros = repositorioTablero.GetAllByUser((int)idUser);
                    return View(tableros);
                }
                return NoContent();
            }


            [HttpGet]
            public IActionResult CreateTablero()
            {
                if(!IsLogged()) return RedirectToAction("Login/Index");
                if(!IsAdmin()) return RedirectToRoute(new { controller = "Home", action = "Index" });
                AltaTableroViewModel altaTableroViewModel = new();
                altaTableroViewModel.Usuarios = repositorioUser.GetAll();
                return View(altaTableroViewModel);
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
            // if(!IsAdmin()) return RedirectToRoute(new { controller = "Home", action = "Index" });
            UpdateTableroViewModel updateTableroViewModel = new();
            updateTableroViewModel.Usuarios = repositorioUser.GetAll();
            updateTableroViewModel.Tablero = tableros.FirstOrDefault(tablero => tablero.Id == idTablero);
            return View(updateTableroViewModel);
            }

            [HttpPost]
            public IActionResult? UpdateTablero(Tablero tablero)
            {
                if(!IsLogged()) return RedirectToAction("Login/Index");
                // if(!IsAdmin()) return RedirectToRoute(new { controller = "Home", action = "Index" });
                repositorioTablero.Update(tablero, tablero.Id);
                return RedirectToAction("Index");
            }

            public IActionResult DeleteTablero(int idTablero)
            {
                if(!IsLogged()) return RedirectToAction("Login/Index");
                // if(!IsAdmin()) return RedirectToRoute(new { controller = "Home", action = "Index" });
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
                if (HttpContext.Session.GetString("Rol") == "Operador") return true;
                return false;
            }

            [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
            public IActionResult Error()
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

        }
}
