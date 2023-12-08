using System.ComponentModel.DataAnnotations;
using tl2_tp10_2023_franciscojvicente.Models;

namespace tl2_tp10_2023_franciscojvicente.ViewModel {
    public class TableroIDViewModel {
        public int Id { get; set; }
        public TableroIDViewModel() {
            
        }
        
        public TableroIDViewModel(Tablero tablero) {
            Id = tablero.Id;
        }
    }
}