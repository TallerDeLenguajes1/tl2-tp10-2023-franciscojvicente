using tl2_tp10_2023_franciscojvicente.Models;
namespace tl2_tp10_2023_franciscojvicente.ViewModel {
    public class LoginUsuarioViewModel
    {
        public int Id { get; set; }
        public string? NombreDeUsuario { get; set; }
        public Roles? Rol { get; set; } 

        public LoginUsuarioViewModel(Usuario usuario) {
            Id = usuario.Id;
            NombreDeUsuario = usuario.NombreDeUsuario;
            Rol = usuario.Rol;
        }        
    
    }
}