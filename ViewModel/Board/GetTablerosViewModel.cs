using tl2_tp10_2023_franciscojvicente.Models;

namespace tl2_tp10_2023_franciscojvicente.ViewModel {
    public class GetTablerosViewModel
    {
        public List<TableroViewModel>? Tableros { get; set; }

        public GetTablerosViewModel(List<TableroViewModel> tableros) {
            Tableros = tableros;
        }
    }
}