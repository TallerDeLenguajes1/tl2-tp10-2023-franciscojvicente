using tl2_tp10_2023_franciscojvicente.Models;

namespace tl2_tp10_2023_franciscojvicente.ViewModel
{
    public class GetTareasViewModel
    {
        public List<TaskViewModel>? Tareas { get; set; }

        public GetTareasViewModel(List<TaskViewModel> tareas) {
            Tareas = tareas;
        }
    }
}