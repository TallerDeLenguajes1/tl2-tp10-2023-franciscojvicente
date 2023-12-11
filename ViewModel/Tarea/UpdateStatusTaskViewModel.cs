using tl2_tp10_2023_franciscojvicente.Models;
using System.ComponentModel.DataAnnotations;

namespace tl2_tp10_2023_franciscojvicente.ViewModel {
    public class UpdateStatusTaskViewModel {
        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo es requerido.")]
        [Display(Name = "Estado")] 
        public StatusTask EstadoTarea { get; set; }

        public UpdateStatusTaskViewModel(int id, StatusTask statusTask) {
            Id = id;
            EstadoTarea = statusTask;
        }

        public UpdateStatusTaskViewModel() {
            
        }
    }
}