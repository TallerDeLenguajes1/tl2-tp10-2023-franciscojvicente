using System.ComponentModel.DataAnnotations;
using tl2_tp10_2023_franciscojvicente.Models;

namespace tl2_tp10_2023_franciscojvicente.ViewModel {
    public class GetUsuariosViewModel
    {
        public List<UsuarioViewModel>? Usuarios { get; set; }

        public GetUsuariosViewModel(List<UsuarioViewModel> usuarios) {
            Usuarios = usuarios;
        }
        public GetUsuariosViewModel() {
        }
    }
}