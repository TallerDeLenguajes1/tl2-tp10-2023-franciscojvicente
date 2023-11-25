using tl2_tp10_2023_franciscojvicente.Models;
using System.ComponentModel.DataAnnotations;

namespace tl2_tp10_2023_franciscojvicente.ViewModel {
    public class UpdateTareaViewModel
        {
            private List<Usuario>? usuarios;
            private List<Tablero>? tableros;
            private int id;
            private int idTablero;
            private string? nombre;
            private string? descripcion;
            private string? color;
            private int id_usuario_asignado;

            [Display(Name = "Id del tablero")]
            public int IdTablero { get => idTablero; set => idTablero = value; }

            [Required(ErrorMessage = "El nombre es requerido.")]
            [Display(Name = "Nombre")] 
            public string? Nombre { get => nombre; set => nombre = value; }

            [Required(ErrorMessage = "Este campo es requerido.")]
            [Display(Name = "Estado")] 
            public EstadoTarea EstadoTarea { get => estadoTarea; set => estadoTarea = value; }

            [Required(ErrorMessage = "La descripción es requerida.")]
            [Display(Name = "Descripción")] 
            public string? Descripcion { get => descripcion; set => descripcion = value; }

            [Required(ErrorMessage = "El color es requerido.")]
            [Display(Name = "Color")] 
            public string? Color { get => color; set => color = value; }

            [Display(Name = "Id del usuario asignado")] 
            public int Id_usuario_asignado { get => id_usuario_asignado; set => id_usuario_asignado = value; }
            private EstadoTarea estadoTarea;

            public List<Usuario>? Usuarios { get => usuarios; set => usuarios = value; }
            public List<Tablero>? Tableros { get => tableros; set => tableros = value; }
            public int Id { get => id; set => id = value; }

            public UpdateTareaViewModel(Tarea tarea)
            {
                Usuarios = new List<Usuario>();
                Tableros = new List<Tablero>();
                Id = tarea.Id;
                IdTablero = tarea.IdTablero;
                Nombre = tarea.Nombre;
                EstadoTarea = tarea.EstadoTarea;
                Descripcion = tarea.Descripcion;
                Color = tarea.Color;
                Id_usuario_asignado = tarea.Id_usuario_asignado;
            }
            
            public UpdateTareaViewModel() {
            }

        }
}