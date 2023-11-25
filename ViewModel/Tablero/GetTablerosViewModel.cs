using tl2_tp10_2023_franciscojvicente.Models;

namespace tl2_tp10_2023_franciscojvicente.ViewModel {
    public class GetTablerosViewModel
    {
        private List<Tablero>? tableros;

        public List<Tablero>? Tableros { get => tableros; set => tableros = value; }

        public GetTablerosViewModel(List<Tablero> tableros) {
            this.tableros = tableros;
        }
    }
}