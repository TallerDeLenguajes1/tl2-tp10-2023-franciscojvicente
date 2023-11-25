using System.ComponentModel.DataAnnotations;
using tl2_tp10_2023_franciscojvicente.Models;

namespace tl2_tp10_2023_franciscojvicente.ViewModel {
    public class GetUsuariosViewModel
    {
        
        private List<Usuario>? usuarios;
        public List<Usuario>? Usuarios { get => usuarios; set => usuarios = value; }

        public GetUsuariosViewModel(List<Usuario> usuarios) {
            this.usuarios = usuarios;
        }
        public GetUsuariosViewModel() {
        }
    }
}