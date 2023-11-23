using tl2_tp10_2023_franciscojvicente.Models;

namespace tl2_tp10_2023_franciscojvicente.ViewModel {
    public class UpdateTareaViewModel
        {
            private List<Usuario>? usuarios;
            private List<Tablero>? tableros;
            private Tarea? tarea;
            private EstadoTarea estadoTarea;

            public List<Usuario>? Usuarios { get => usuarios; set => usuarios = value; }
            public List<Tablero>? Tableros { get => tableros; set => tableros = value; }
            public Tarea? Tarea { get => tarea; set => tarea = value; }
            public EstadoTarea EstadoTarea { get => estadoTarea; set => estadoTarea = value; }

            public UpdateTareaViewModel()
            {
                Usuarios = new List<Usuario>();
                Tableros = new List<Tablero>();
                Tarea = new Tarea();
                EstadoTarea = new EstadoTarea();
            }
        }
}