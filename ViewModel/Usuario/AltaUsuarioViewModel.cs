using System.ComponentModel.DataAnnotations;
using tl2_tp10_2023_franciscojvicente.Models;

namespace tl2_tp10_2023_franciscojvicente.ViewModel {
    public class AltaUsuarioViewModel
    {
        private string? nombreDeUsuario;
        private Roles? rol;
        private string? contrasenia;

        
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [Display(Name = "Nombre de usuario")] 
        public string? NombreDeUsuario { get => nombreDeUsuario; set => nombreDeUsuario = value; }

        
        [Required(ErrorMessage = "El rol es obligatorio.")]
        [Display(Name = "Rol")] 
        public Roles? Rol { get => rol; set => rol = value; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [Display(Name = "Contraseña")] 
        public string? Contrasenia { get => contrasenia; set => contrasenia = value; }

        public AltaUsuarioViewModel(Usuario usuario) {
            NombreDeUsuario = usuario.NombreDeUsuario;
            Rol = usuario.Rol;
            Contrasenia = usuario.Contrasenia;
        }
        public AltaUsuarioViewModel() {
        }
    }
}