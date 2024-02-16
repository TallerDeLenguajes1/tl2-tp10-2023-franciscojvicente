using System.ComponentModel.DataAnnotations;
using tl2_tp10_2023_franciscojvicente.Models;

namespace tl2_tp10_2023_franciscojvicente.ViewModel {
    public class TableroNameViewModel {
        public int Id { get; set; }
        public string? Name { get; set; }
        public TableroNameViewModel() {
            
        }
        
        public TableroNameViewModel(Tablero tablero) {
            Id = tablero.Id;
            Name = tablero.Nombre;
        }
    }
}