using System.ComponentModel.DataAnnotations;
using tl2_tp10_2023_franciscojvicente.Models;

namespace tl2_tp10_2023_franciscojvicente.ViewModel {
    public class GetUsuariosViewModel
    {
        public List<Usuario>? Usuarios { get; set; }

        public GetUsuariosViewModel(List<Usuario> usuarios) {
            Usuarios = usuarios;
        }
        public GetUsuariosViewModel() {
        }
    }
}