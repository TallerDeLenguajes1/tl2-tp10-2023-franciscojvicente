using tl2_tp10_2023_franciscojvicente.Models;
using System.ComponentModel.DataAnnotations;
// using MVC.Models;

namespace tl2_tp10_2023_franciscojvicente.ViewModel {
    public class AltaTableroViewModel
    {
        private List<Usuario>? usuarios;
        private int id_usuario_propietario;
        private string? nombre;
        private string? descripcion;


        [Display(Name = "Id del usuario propietario")] 
        public int Id_usuario_propietario { get => id_usuario_propietario; set => id_usuario_propietario = value; }

        [Required(ErrorMessage = "Este campo es requerido.")]
        [Display(Name = "Nombre")] 
        public string? Nombre { get => nombre; set => nombre = value; }

        [Required(ErrorMessage = "Este campo es requerido.")]
        [Display(Name = "Descripción")] 
        public string? Descripcion { get => descripcion; set => descripcion = value; }
        public List<Usuario>? Usuarios { get => usuarios; set => usuarios = value; }

        public AltaTableroViewModel(Tablero tablero)
        {
            this.Usuarios = new List<Usuario>();
            Id_usuario_propietario = tablero.Id_usuario_propietario;
            Nombre = tablero.Nombre;
            Descripcion = tablero.Descripcion;
        }
        public AltaTableroViewModel() {
        }
    }
}