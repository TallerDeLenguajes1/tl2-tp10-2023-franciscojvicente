using tl2_tp10_2023_franciscojvicente.Models;

namespace tl2_tp10_2023_franciscojvicente.ViewModel
{
    public class GetTareasViewModel
    {
        private List<Tarea>? tareas;

        public List<Tarea>? Tareas { get => tareas; set => tareas = value; }

        public GetTareasViewModel() {
            Tareas = new();
        }
    }
}