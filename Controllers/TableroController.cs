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
            private readonly ITableroRepository _tableroRepository;
            private readonly IUsuarioRepository _usuarioRepository;

            public TableroController(ITableroRepository tableroRepository, IUsuarioRepository usuarioRepository) {
                    _tableroRepository = tableroRepository;
                    _usuarioRepository = usuarioRepository;
            }
            
            // readonly TableroRepository _tableroRepository = new();
            // readonly UsuarioRepository _usuarioRepository = new();
            // readonly GetTablerosViewModel getTablerosViewModel = new();
            private static List<Tablero> tableros = new();
            
            public IActionResult Index()
            {
                if (!IsLogged()) return RedirectToAction("Login", "Index");

                if (IsAdmin())
                {
                    // getTablerosViewModel.Tableros = _tableroRepository.GetAll();
                    tableros = _tableroRepository.GetAll();
                    return View(tableros);
                }
                if (IsOperator())
                {
                    var idUser = HttpContext.Session.GetInt32("Id");
                    if(idUser == null) return NoContent();
                    // getTablerosViewModel.Tableros = _tableroRepository.GetAllByUser((int)idUser);
                    tableros = _tableroRepository.GetAllByUser((int)idUser);
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
                altaTableroViewModel.Usuarios = _usuarioRepository.GetAll();
                return View(altaTableroViewModel);
            }

            [HttpPost]
            public IActionResult? CreateTablero(Tablero tablero)
            {
                if(!IsLogged()) return RedirectToAction("Login/Index");
                if(!IsAdmin()) return RedirectToRoute(new { controller = "Home", action = "Index" });
                if(!ModelState.IsValid) return RedirectToAction("Index");
                if (tablero == null) return null;
                _tableroRepository.Create(tablero);
                return RedirectToAction("Index");
            }

            [HttpGet]
            public IActionResult UpdateTablero(int idTablero)
            {
            if(!IsLogged()) return RedirectToAction("Login/Index");
            UpdateTableroViewModel updateTableroViewModel = new();
            updateTableroViewModel.Usuarios = _usuarioRepository.GetAll();
            updateTableroViewModel.Tablero = tableros.FirstOrDefault(tablero => tablero.Id == idTablero);
            return View(updateTableroViewModel);
            }

            [HttpPost]
            public IActionResult? UpdateTablero(Tablero tablero)
            {
                if(!IsLogged()) return RedirectToAction("Login/Index");
                if(!IsLogged()) return RedirectToAction("Login/Index");
                if(!ModelState.IsValid) return RedirectToAction("Index");
                _tableroRepository.Update(tablero, tablero.Id);
                return RedirectToAction("Index");
            }

            public IActionResult DeleteTablero(int idTablero)
            {
                if(!IsLogged()) return RedirectToAction("Login/Index");
                // if(!IsAdmin()) return RedirectToRoute(new { controller = "Home", action = "Index" });
                _tableroRepository.Delete(idTablero);
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
