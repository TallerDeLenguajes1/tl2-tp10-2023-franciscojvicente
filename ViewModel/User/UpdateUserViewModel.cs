using System.ComponentModel.DataAnnotations;
using tl2_tp10_2023_franciscojvicente.Models;

namespace tl2_tp10_2023_franciscojvicente.ViewModel {
    public class UpdateUserViewModel
    {
 
        [Required(ErrorMessage = "El nombre de usuario es requerido.")]
        [Display(Name = "Nombre de usuario")] 
        public string? NombreDeUsuario { get; set; }

        
        [Required(ErrorMessage = "El rol es requerido.")]
        [Display(Name = "Rol")] 
        public Roles? Rol { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida.")]
        [Display(Name = "Contraseña")] 
        public string? Contrasenia { get; set; }
        public int Id { get; set; }

        public int IdLogueado { get; set; }

        public UpdateUserViewModel(Usuario usuario) {
            if (usuario.Id == 0 || usuario.NombreDeUsuario == null || usuario.Rol == null || usuario.Contrasenia == null) new Usuario();
            Id = usuario.Id;
            NombreDeUsuario = usuario.NombreDeUsuario;
            Rol = usuario.Rol;
            Contrasenia = usuario.Contrasenia;
        }
        public UpdateUserViewModel() {
        }
    }
}