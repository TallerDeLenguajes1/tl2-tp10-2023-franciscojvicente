using tl2_tp10_2023_franciscojvicente.Models;

namespace tl2_tp10_2023_franciscojvicente.ViewModel {
    public class GetTasksInBoardViewModel
    {
        public List<Tarea>? Tareas { get; set; }
        public int IdOwnerBoard { get; set; }

        public GetTasksInBoardViewModel(List<Tarea> tareas) {
            Tareas = tareas;
        }
    }
}