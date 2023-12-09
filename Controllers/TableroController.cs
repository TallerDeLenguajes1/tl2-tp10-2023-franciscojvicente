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
        private readonly ILogger<HomeController> _logger;

        public TableroController(ITableroRepository tableroRepository, IUsuarioRepository usuarioRepository, ILogger<HomeController> logger) {
                _tableroRepository = tableroRepository;
                _usuarioRepository = usuarioRepository;
                _logger = logger;
        }
        
        public IActionResult Index()
        {
            try
            {
                if (!IsLogged()) return RedirectToRoute(new {controller = "Login", action = "Index"});
                if (IsOperator())
                {
                    var idUser = HttpContext.Session.GetInt32("Id");
                    if(idUser == null) return NoContent();
                    var tableros = _tableroRepository.GetAllByUser((int)idUser);
                    var getTablerosViewModel = new GetTablerosViewModel(tableros);
                    return View(getTablerosViewModel);
                }
                if (IsAdmin())
                {
                    var tableros = _tableroRepository.GetAll();
                    var getTablerosViewModel = new GetTablerosViewModel(tableros);
                    return View(getTablerosViewModel);
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
        public IActionResult CreateTablero()
        {
            try
            {
                if(!IsLogged()) return RedirectToRoute(new {controller = "Home", action = "Index"});
                // if(!IsAdmin()) return RedirectToRoute(new { controller = "Home", action = "Index" });
                AltaTableroViewModel altaTableroViewModel = new();
                altaTableroViewModel.Usuarios = _usuarioRepository.GetAllID();
                if (altaTableroViewModel.Usuarios == null) return NoContent();
                return View(altaTableroViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return RedirectToRoute(new {controller = "Home", action = "Index"});
            }
        }

        [HttpPost]
        public IActionResult? CreateTablero(AltaTableroViewModel altaTableroViewModel)
        {
            try
            {    
                if(!IsLogged()) return RedirectToRoute(new {controller = "Home", action = "Index"});
                // if(!IsAdmin()) return RedirectToRoute(new { controller = "Home", action = "Index" });
                // if(!ModelState.IsValid) return RedirectToAction("Index");
                var tablero = new Tablero(altaTableroViewModel);
                if (tablero == null) return null;
                _tableroRepository.Create(tablero);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return RedirectToRoute(new {controller = "Home", action = "Index"});
            }
        }

        [HttpGet]
        public IActionResult UpdateTablero(int idTablero)
        {
            try
            {
                if(!IsLogged()) return RedirectToAction("Login/Index");
                var tablero = _tableroRepository.GetById(idTablero);
                if (tablero == null) return NoContent();
                UpdateTableroViewModel updateTableroViewModel = new(tablero);
                updateTableroViewModel.Usuarios = _usuarioRepository.GetAllID();
                return View(updateTableroViewModel);    
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return RedirectToRoute(new {controller = "Home", action = "Index"});
            }
        }

        [HttpPost]
        public IActionResult? UpdateTablero(UpdateTableroViewModel updateTableroViewModel)
        {
            try
            {
                if(!IsLogged()) return RedirectToAction("Login/Index");
                // if(!ModelState.IsValid) return RedirectToAction("Index");
                var tablero = new Tablero(updateTableroViewModel);
                _tableroRepository.Update(tablero, tablero.Id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return RedirectToRoute(new {controller = "Home", action = "Index"});              
            }
        }

        public IActionResult DeleteTablero(int idTablero)
        {
            try
            {
                if(!IsLogged()) return RedirectToAction("Login/Index");
                _tableroRepository.Delete(idTablero);
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
            if (HttpContext.Session.GetString("Rol") == "Operador") return true;
            return false;
        }

        // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        // public IActionResult Error()
        // {
        //     return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        // }
    }
}
