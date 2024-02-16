using tl2_tp10_2023_franciscojvicente.Models;
using System.ComponentModel.DataAnnotations;

namespace tl2_tp10_2023_franciscojvicente.ViewModel {
    public class CreateBoardViewModel
    {

        [Display(Name = "Usuario propietario")] 
        public int Id_usuario_propietario { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [Display(Name = "Nombre")] 
        public string? Nombre { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [Display(Name = "Descripción")] 
        public string? Descripcion { get; set; }
        public List<UsuarioNameViewModel>? Usuarios { get; set; }

        public CreateBoardViewModel(Tablero tablero)
        {
            Usuarios = new List<UsuarioNameViewModel>();
            Id_usuario_propietario = tablero.Id_usuario_propietario;
            Nombre = tablero.Nombre;
            Descripcion = tablero.Descripcion;
        }
        public CreateBoardViewModel() {
        }
    }
}