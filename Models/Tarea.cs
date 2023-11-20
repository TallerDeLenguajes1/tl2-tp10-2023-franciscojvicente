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
    }
}