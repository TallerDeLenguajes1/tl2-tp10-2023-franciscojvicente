using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace tl2_tp10_2023_franciscojvicente.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El nombre de usuario es requerido.")]
        [Display(Name = "Nombre de Usuario")] 
        public string? NombreDeUsuario { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida.")]
        [PasswordPropertyText(true)]
        [Display(Name = "Contraseña")]
        public string? Contrasenia { get; set; }
    }
}