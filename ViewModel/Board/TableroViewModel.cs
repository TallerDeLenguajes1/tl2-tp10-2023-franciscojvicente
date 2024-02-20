using tl2_tp10_2023_franciscojvicente.Models;

namespace tl2_tp10_2023_franciscojvicente.ViewModel {
    public class TableroViewModel
    {

        public int Id { get; set; }
        public int Id_usuario_propietario { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public string? NombreProp  { get; set; }

        public TableroViewModel(Tablero tablero)
        {
            Id = tablero.Id;
            Id_usuario_propietario = tablero.Id_usuario_propietario;
            Nombre = tablero.Nombre;
            Descripcion = tablero.Descripcion;
        }
        public TableroViewModel() {
        }
    }
}