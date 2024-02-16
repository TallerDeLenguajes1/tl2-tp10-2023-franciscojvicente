using System.ComponentModel.DataAnnotations;
using tl2_tp10_2023_franciscojvicente.Models;

namespace tl2_tp10_2023_franciscojvicente.ViewModel {
    public class UsuarioNameViewModel {
        public string? Name { get; set; }
        public int Id { get; set; }
        public UsuarioNameViewModel() {
            
        }
        
        public UsuarioNameViewModel(Usuario usuario) {
            Name = usuario.NombreDeUsuario;
            Id = usuario.Id;
        }
    }
}