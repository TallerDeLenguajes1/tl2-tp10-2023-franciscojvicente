using tl2_tp10_2023_franciscojvicente.Models;

namespace tl2_tp10_2023_franciscojvicente.ViewModel
{
    public class GetTareasViewModel
    {
        public List<Tarea>? Tareas { get; set; }

        public GetTareasViewModel(List<Tarea> tareas) {
            Tareas = tareas;
        }
    }
}