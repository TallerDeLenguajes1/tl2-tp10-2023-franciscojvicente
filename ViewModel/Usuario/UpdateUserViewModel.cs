using System.ComponentModel.DataAnnotations;
using tl2_tp10_2023_franciscojvicente.Models;

namespace tl2_tp10_2023_franciscojvicente.ViewModel {
    public class UpdateUserViewModel
    {
        private int id;
        private string? nombreDeUsuario;
        private Roles? rol;
        private string? contrasenia;

        
        [Required(ErrorMessage = "Este campo es requerido.")]
        [Display(Name = "Nombre de usuario")] 
        public string? NombreDeUsuario { get => nombreDeUsuario; set => nombreDeUsuario = value; }

        
        [Required(ErrorMessage = "Este campo es requerido.")]
        [Display(Name = "Rol")] 
        public Roles? Rol { get => rol; set => rol = value; }

        [Required(ErrorMessage = "Este campo es requerido.")]
        [Display(Name = "Contraseña")] 
        public string? Contrasenia { get => contrasenia; set => contrasenia = value; }
        public int Id { get => id; set => id = value; }

        public UpdateUserViewModel(Usuario usuario) {
            Id = usuario.Id;
            NombreDeUsuario = usuario.NombreDeUsuario;
            Rol = usuario.Rol;
            Contrasenia = usuario.Contrasenia;
        }
        public UpdateUserViewModel() {
        }
    }
}