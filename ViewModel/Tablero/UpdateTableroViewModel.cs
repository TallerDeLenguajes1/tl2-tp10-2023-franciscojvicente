using tl2_tp10_2023_franciscojvicente.Models;
using System.ComponentModel.DataAnnotations;
// using MVC.Models;

namespace tl2_tp10_2023_franciscojvicente.ViewModel {
    public class UpdateTableroViewModel
    {
        private List<Usuario>? usuarios;
        // private Tablero? tablero;
        private int id;
        private int id_usuario_propietario;
        private string? nombre;
        private string? descripcion;
        public List<Usuario>? Usuarios { get => usuarios; set => usuarios = value; }
        public int Id { get => id; set => id = value; }
        
        [Display(Name = "Id del usuario propietario")] 
        public int Id_usuario_propietario { get => id_usuario_propietario; set => id_usuario_propietario = value; }
        
        [Required(ErrorMessage = "El nombre es requerido.")]
        [Display(Name = "Nombre")] 
        public string? Nombre { get => nombre; set => nombre 
        = value; }
        [Required(ErrorMessage = "La descripción es requerida.")]
        [Display(Name = "Descripción")] 
        public string? Descripcion { get => descripcion; set => descripcion = value; }

        // public Tablero? Tablero { get => tablero; set => tablero = value; }

        public UpdateTableroViewModel(Tablero tablero)
        {
            Usuarios = new List<Usuario>();
            Id = tablero.Id;
            Id_usuario_propietario = tablero.Id_usuario_propietario;
            Nombre = tablero.Nombre;
            Descripcion = tablero.Descripcion;
        }

        public UpdateTableroViewModel() {
            
        }
        // public UpdateTableroViewModel(Tablero tablero)
        // {
        //     Usuarios = new List<Usuario>();
        //     Id = tablero.Id;
        //     Id_usuario_propietario = tablero.Id_usuario_propietario;
        //     Nombre = tablero.Nombre;
        //     Descripcion = tablero.Descripcion;
        // }

    }
}