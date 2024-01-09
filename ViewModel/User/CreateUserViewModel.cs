using System.ComponentModel.DataAnnotations;
using tl2_tp10_2023_franciscojvicente.Models;

namespace tl2_tp10_2023_franciscojvicente.ViewModel {
    public class CreateUserViewModel
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [Display(Name = "Nombre de usuario")] 
        public string? NombreDeUsuario { get; set; }

        
        [Required(ErrorMessage = "El rol es obligatorio.")]
        [Display(Name = "Rol")] 
        public Roles? Rol { get ; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [Display(Name = "Contraseña")] 
        public string? Contrasenia { get; set; }

        public CreateUserViewModel(Usuario usuario) {
            NombreDeUsuario = usuario.NombreDeUsuario;
            Rol = usuario.Rol;
            Contrasenia = usuario.Contrasenia;
        }
        public CreateUserViewModel() {
        }
    }
}