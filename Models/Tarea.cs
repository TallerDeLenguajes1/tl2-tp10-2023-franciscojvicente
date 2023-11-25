using tl2_tp10_2023_franciscojvicente.ViewModel;
namespace tl2_tp10_2023_franciscojvicente.Models
{
    public class Tarea
    {
        private int id;
        private int idTablero;
        private string? nombre;
        private EstadoTarea estadoTarea;
        private string? descripcion;
        private string? color;
        private int id_usuario_asignado;

        public int Id { get => id; set => id = value; }
        public int IdTablero { get => idTablero; set => idTablero = value; }
        public string? Nombre { get => nombre; set => nombre = value; }
        public EstadoTarea EstadoTarea { get => estadoTarea; set => estadoTarea = value; }
        public string? Descripcion { get => descripcion; set => descripcion = value; }
        public string? Color { get => color; set => color = value; }
        public int Id_usuario_asignado { get => id_usuario_asignado; set => id_usuario_asignado = value; }
        public Tarea(AltaTareaViewModel altaTareaViewModel) {
            IdTablero = altaTareaViewModel.IdTablero;
            Nombre = altaTareaViewModel.Nombre;
            EstadoTarea = altaTareaViewModel.EstadoTarea;
            Descripcion = altaTareaViewModel.Descripcion;
            Color = altaTareaViewModel.Color;
            Id_usuario_asignado = altaTareaViewModel.Id_usuario_asignado;
        }
        public Tarea(UpdateTareaViewModel updateTareaViewModel) {
            Id = updateTareaViewModel.Id;
            IdTablero = updateTareaViewModel.IdTablero;
            Nombre = updateTareaViewModel.Nombre;
            EstadoTarea = updateTareaViewModel.EstadoTarea;
            Descripcion = updateTareaViewModel.Descripcion;
            Color = updateTareaViewModel.Color;
            Id_usuario_asignado = updateTareaViewModel.Id_usuario_asignado;
        }
        public Tarea() {
        }
    }
}