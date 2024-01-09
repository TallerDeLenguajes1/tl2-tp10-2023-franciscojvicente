using System.ComponentModel.DataAnnotations;
using tl2_tp10_2023_franciscojvicente.Models;

namespace tl2_tp10_2023_franciscojvicente.ViewModel {
    public class GetUsuariosViewModel
    {
        public List<UserViewModel>? Usuarios { get; set; }

        public GetUsuariosViewModel(List<UserViewModel> usuarios) {
            Usuarios = usuarios;
        }
        public GetUsuariosViewModel() {
        }
    }
}