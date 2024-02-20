using tl2_tp10_2023_franciscojvicente.Models;

namespace tl2_tp10_2023_franciscojvicente.ViewModel {
    public class TaskViewModel {
        public int Id { get; set; }
        public int IdTablero { get; set; }
        public string? Nombre { get; set; }
        public StatusTask EstadoTarea { get; set; } 
        public string? Descripcion { get; set; }
        public string? Color { get; set; }
        public int Id_usuario_asignado { get; set; }
        public string? NombreProp { get; set; }
        public string? OwnerBoard { get; set; }

        public TaskViewModel(Tarea tarea) {
            Id = tarea.Id;
            IdTablero = tarea.IdTablero;
            Nombre = tarea.Nombre;
            EstadoTarea = tarea.EstadoTarea;
            Descripcion = tarea.Descripcion;
            Color = tarea.Color;
            Id_usuario_asignado = tarea.Id_usuario_asignado;
        }
        public TaskViewModel() {
        }
    }
}