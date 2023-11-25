using tl2_tp10_2023_franciscojvicente.ViewModel;
namespace tl2_tp10_2023_franciscojvicente.Models
{
    public class Usuario {
        
        private int id;
        private string? nombreDeUsuario;
        private Roles? rol;
        private string? contrasenia;

        public int Id { get => id; set => id = value;}
        public string? NombreDeUsuario { get => nombreDeUsuario; set => nombreDeUsuario = value;}
        public Roles? Rol { get => rol; set => rol = value; }
        public string? Contrasenia { get => contrasenia; set => contrasenia = value; }

        public Usuario(AltaUsuarioViewModel altaUsuarioViewModel) {
            NombreDeUsuario = altaUsuarioViewModel.NombreDeUsuario;
            Rol = altaUsuarioViewModel.Rol;
            Contrasenia = altaUsuarioViewModel.Contrasenia;
        }

        public Usuario(UpdateUserViewModel updateUserViewModel) {
            Id = updateUserViewModel.Id;
            NombreDeUsuario = updateUserViewModel.NombreDeUsuario;
            Rol = updateUserViewModel.Rol;
            Contrasenia = updateUserViewModel.Contrasenia;
        }
        public Usuario() {

        }
    }
}