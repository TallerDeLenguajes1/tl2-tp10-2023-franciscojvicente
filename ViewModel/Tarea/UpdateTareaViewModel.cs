using tl2_tp10_2023_franciscojvicente.Models;
using System.ComponentModel.DataAnnotations;

namespace tl2_tp10_2023_franciscojvicente.ViewModel {
    public class UpdateTareaViewModel
        {
            [Display(Name = "Id del tablero")]
            public int IdTablero { get; set; }

            [Required(ErrorMessage = "El nombre es requerido.")]
            [Display(Name = "Nombre")] 
            public string? Nombre { get; set; }

            [Required(ErrorMessage = "Este campo es requerido.")]
            [Display(Name = "Estado")] 
            public EstadoTarea EstadoTarea { get; set; }

            [Required(ErrorMessage = "La descripción es requerida.")]
            [Display(Name = "Descripción")] 
            public string? Descripcion { get; set; }

            [Required(ErrorMessage = "El color es requerido.")]
            [Display(Name = "Color")] 
            public string? Color { get; set; }

            [Display(Name = "Id del usuario asignado")] 
            public int Id_usuario_asignado { get; set; }
            public List<UsuarioIDViewModel>? Usuarios { get; set; }
            public List<TableroIDViewModel>? Tableros { get; set; }
            public int Id { get; set; }

            public UpdateTareaViewModel(Tarea tarea)
            {
                Usuarios = new List<UsuarioIDViewModel>();
                Tableros = new List<TableroIDViewModel>();
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