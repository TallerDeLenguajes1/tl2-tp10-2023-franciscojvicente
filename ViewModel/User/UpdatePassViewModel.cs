using System.ComponentModel.DataAnnotations;
using tl2_tp10_2023_franciscojvicente.Models;

namespace tl2_tp10_2023_franciscojvicente.ViewModel {
    public class UpdatePassViewModel { 
        [Required(ErrorMessage = "La contraseña es requerida.")]
        [Display(Name = "Contraseña")] 
        public string? Password { get; set; }

        public int Id { get; set; }
        public UpdatePassViewModel(string pass, int id) {
            Password = pass;
            Id = id;
        }

        public UpdatePassViewModel() {
            
        }
    }
}