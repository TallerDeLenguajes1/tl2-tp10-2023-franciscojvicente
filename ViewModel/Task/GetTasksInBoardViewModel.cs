using tl2_tp10_2023_franciscojvicente.Models;

namespace tl2_tp10_2023_franciscojvicente.ViewModel {
    public class GetTasksInBoardViewModel
    {
        public List<TaskViewModel>? Tareas { get; set; }
        public int IdOwnerBoard { get; set; }
        public int IDBoard { get; set; }

        public GetTasksInBoardViewModel(List<TaskViewModel> tareas) {
            Tareas = tareas;
        }
    }
}