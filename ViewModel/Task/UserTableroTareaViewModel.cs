using tl2_tp10_2023_franciscojvicente.Models;
namespace tl2_tp10_2023_franciscojvicente.ViewModel
{
    public class UserTableroTareaViewModel
    {
        public List<Usuario>? Usuarios { get; set; }
        public List<Tablero>? Tableros { get; set; }
        public Tarea? Tarea { get; set; }
        public StatusTask EstadoTarea { get; set; }

        public UserTableroTareaViewModel()
        {
            Usuarios = new List<Usuario>();
            Tableros = new List<Tablero>();
            Tarea = new Tarea();
            EstadoTarea = new StatusTask();
        }
    }
}