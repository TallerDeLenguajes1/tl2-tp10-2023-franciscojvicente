using tl2_tp10_2023_franciscojvicente.Models;

namespace tl2_tp10_2023_franciscojvicente.ViewModel {
    public class GetTablerosViewModel
    {
        public List<Tablero>? Tableros { get; set; }

        public GetTablerosViewModel(List<Tablero> tableros) {
            Tableros = tableros;
        }
    }
}