using tl2_tp10_2023_franciscojvicente.ViewModel;
namespace tl2_tp10_2023_franciscojvicente.Models
{
    public class Tablero
    {
        private int id;
        private int id_usuario_propietario;
        private string? nombre;
        
        private string? descripcion;

        public int Id { get => id; set => id = value; }
        public int Id_usuario_propietario { get => id_usuario_propietario; set => id_usuario_propietario = value; }
        public string? Nombre { get => nombre; set => nombre = value; }
        public string? Descripcion { get => descripcion; set => descripcion = value; }

        public Tablero(AltaTableroViewModel altaTableroViewModel) {
            Id_usuario_propietario = altaTableroViewModel.Id_usuario_propietario;
            Nombre = altaTableroViewModel.Nombre;
            Descripcion = altaTableroViewModel.Descripcion;
        }

        public Tablero(UpdateTableroViewModel updateTableroViewModel) {
            Id = updateTableroViewModel.Id;
            Id_usuario_propietario = updateTableroViewModel.Id_usuario_propietario;
            Nombre = updateTableroViewModel.Nombre;
            Descripcion = updateTableroViewModel.Descripcion;
        }

        public Tablero() {
            
        }
    }
}