using tl2_tp10_2023_franciscojvicente.Models;
using System.ComponentModel.DataAnnotations;
// using MVC.Models;

namespace tl2_tp10_2023_franciscojvicente.ViewModel {
    public class UsuarioTableroViewModel
    {
        private List<Usuario>? usuarios;
        private Tablero? tablero;

        public List<Usuario>? Usuarios { get; set; }

        [Required]        
        public Tablero? Tablero { get; set; }

        public UsuarioTableroViewModel()
        {
            this.Usuarios = new List<Usuario>();
            this.Tablero = new Tablero();
        }
    }
}