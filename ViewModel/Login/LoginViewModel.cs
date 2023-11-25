using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace tl2_tp10_2023_franciscojvicente.ViewModel
{
    public class LoginViewModel
    {
        private string? nombreDeUsuario;
        private string? contrasenia;

        [Required(ErrorMessage = "El nombre de usuario es requerido.")]
        [Display(Name = "Nombre de Usuario")] 
        public string? NombreDeUsuario { get => nombreDeUsuario; set => nombreDeUsuario = value; }

        [Required(ErrorMessage = "La contraseña es requerida.")]
        [PasswordPropertyText(true)]
        [Display(Name = "Contraseña")]
        public string? Contrasenia { get => contrasenia; set => contrasenia = value; }
    }
}