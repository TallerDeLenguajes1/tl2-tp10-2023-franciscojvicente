using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp10_2023_franciscojvicente.Models;
using tl2_tp10_2023_franciscojvicente.Repository;

namespace tl2_tp10_2023_franciscojvicente.Controllers
{
    using System;
    using Microsoft.AspNetCore.Mvc;
    
        public class LoginController : Controller
        {
            public IActionResult Index()
            {
                return View();
            }

            public bool IsLogged()
            {
            if (HttpContext.Session != null) return true;
            return false;
            }

        public bool IsAdmin()
        {
            if (HttpContext.Session != null && HttpContext.Session.GetString("rol") == "Administrador") return true;
            return false;
        }

        public bool IsOperator()
        {
            if (HttpContext.Session != null && HttpContext.Session.GetString("rol") == "Operador") return true;
            return false;
        }
    }
}

