using tl2_tp10_2023_franciscojvicente.Models;
using System.ComponentModel.DataAnnotations;

namespace tl2_tp10_2023_franciscojvicente.ViewModel {
    public class UpdateTaskViewModel
    {
        [Display(Name = "Tablero")]
        public int IdTablero { get; set; }

        [Required(ErrorMessage = "El nombre es requerido.")]
        [Display(Name = "Nombre")] 
        public string? Nombre { get; set; }

        [Required(ErrorMessage = "Este campo es requerido.")]
        [Display(Name = "Estado")] 
        public StatusTask EstadoTarea { get; set; }

        [Required(ErrorMessage = "La descripción es requerida.")]
        [Display(Name = "Descripción")] 
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El color es requerido.")]
        [Display(Name = "Color")] 
        public string? Color { get; set; }

        [Display(Name = "Usuario asignado")] 
        public int Id_usuario_asignado { get; set; }
        public List<UsuarioNameViewModel>? Usuarios { get; set; }
        public List<TableroNameViewModel>? Tableros { get; set; }
        public int Id { get; set; }

        public UpdateTaskViewModel(Tarea tarea)
        {
            Usuarios = new List<UsuarioNameViewModel>();
            Tableros = new List<TableroNameViewModel>();
            Id = tarea.Id;
            IdTablero = tarea.IdTablero;
            Nombre = tarea.Nombre;
            EstadoTarea = tarea.EstadoTarea;
            Descripcion = tarea.Descripcion;
            Color = tarea.Color;
            Id_usuario_asignado = tarea.Id_usuario_asignado;
        }
        
        public UpdateTaskViewModel() {
        }

    }
}