using tl2_tp10_2023_franciscojvicente.Models;
using System.ComponentModel.DataAnnotations;
// using MVC.Models;

namespace tl2_tp10_2023_franciscojvicente.ViewModel {
    public class UpdateTableroViewModel
    {
        public List<Usuario>? Usuarios { get; set; }
        public int Id { get; set; }
        
        [Display(Name = "Id del usuario propietario")] 
        public int Id_usuario_propietario { get; set; }
        
        [Required(ErrorMessage = "El nombre es requerido.")]
        [Display(Name = "Nombre")] 
        public string? Nombre { get; set; }
        [Required(ErrorMessage = "La descripción es requerida.")]
        [Display(Name = "Descripción")] 
        public string? Descripcion { get; set; }

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
    }
}