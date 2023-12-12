using tl2_tp10_2023_franciscojvicente.Models;
using System.ComponentModel.DataAnnotations;

namespace tl2_tp10_2023_franciscojvicente.ViewModel {
    public class AltaTableroViewModel
    {

        [Display(Name = "Id del usuario propietario")] 
        public int Id_usuario_propietario { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [Display(Name = "Nombre")] 
        public string? Nombre { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [Display(Name = "Descripción")] 
        public string? Descripcion { get; set; }
        public List<UsuarioIDViewModel>? Usuarios { get; set; }

        public AltaTableroViewModel(Tablero tablero)
        {
            Usuarios = new List<UsuarioIDViewModel>();
            Id_usuario_propietario = tablero.Id_usuario_propietario;
            Nombre = tablero.Nombre;
            Descripcion = tablero.Descripcion;
        }
        public AltaTableroViewModel() {
        }
    }
}