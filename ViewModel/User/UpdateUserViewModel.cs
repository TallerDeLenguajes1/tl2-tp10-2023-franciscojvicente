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
        public int Id { get; set; }

        public int IdLogueado { get; set; }

        public UpdateUserViewModel(string username, Roles rol, int id) {
            if (username is null || id == 0) new Usuario();
            Id = id;
            NombreDeUsuario = username;
            Rol = rol;
        }
        public UpdateUserViewModel() {
        }
    }
}