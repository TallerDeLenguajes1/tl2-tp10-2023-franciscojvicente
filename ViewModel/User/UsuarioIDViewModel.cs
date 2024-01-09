using System.ComponentModel.DataAnnotations;
using tl2_tp10_2023_franciscojvicente.Models;

namespace tl2_tp10_2023_franciscojvicente.ViewModel {
    public class UsuarioIDViewModel {
        public int Id { get; set; }
        public UsuarioIDViewModel() {
            
        }
        
        public UsuarioIDViewModel(Usuario usuario) {
            Id = usuario.Id;
        }
    }
}