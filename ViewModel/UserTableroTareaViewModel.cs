namespace tl2_tp10_2023_franciscojvicente.Models
{
    public class UserTableroTareaViewModel
    {
        private List<Usuario>? usuarios;
        private List<Tablero>? tableros;
        private Tarea? tarea;
        private EstadoTarea estadoTarea;

        public List<Usuario>? Usuarios { get => usuarios; set => usuarios = value; }
        public List<Tablero>? Tableros { get => tableros; set => tableros = value; }
        public Tarea? Tarea { get => tarea; set => tarea = value; }
        public EstadoTarea EstadoTarea { get => estadoTarea; set => estadoTarea = value; }

        public UserTableroTareaViewModel()
        {
            this.Usuarios = new List<Usuario>();
            this.Tableros = new List<Tablero>();
            this.Tarea = new Tarea();
            this.EstadoTarea = new EstadoTarea();
        }
    }
}