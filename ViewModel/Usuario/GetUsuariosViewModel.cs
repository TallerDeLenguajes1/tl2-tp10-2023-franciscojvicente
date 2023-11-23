using System.ComponentModel.DataAnnotations;
using tl2_tp10_2023_franciscojvicente.Models;

namespace tl2_tp10_2023_franciscojvicente.ViewModel {
    public class GetUsuariosViewModel
    {
        private int id;
        private string? nombreDeUsuario;
        private Roles? rol;
        private List<GetUsuariosViewModel>? usuarios;

        public string? NombreDeUsuario { get => nombreDeUsuario; set => nombreDeUsuario = value; }
        public Roles? Rol { get => rol; set => rol = value; }
        public int Id { get => id; set => id = value; }
        public List<GetUsuariosViewModel>? Usuarios { get => usuarios; set => usuarios = value; }

        public GetUsuariosViewModel(Usuario usuario) {
            Id = usuario.Id;
            NombreDeUsuario = usuario.NombreDeUsuario;
            Rol = usuario.Rol;
            Usuarios = new List<GetUsuariosViewModel>();
        }
        public GetUsuariosViewModel() {
        }
    }
}