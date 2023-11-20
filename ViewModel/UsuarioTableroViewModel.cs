namespace tl2_tp10_2023_franciscojvicente.Models {
    public class UsuarioTableroViewModel
    {
        private List<Usuario>? usuarios;
        private Tablero? tablero;

        public List<Usuario>? Usuarios { get; set; }
        public Tablero? Tablero { get; set; }

        public UsuarioTableroViewModel()
        {
            this.Usuarios = new List<Usuario>();
            this.Tablero = new Tablero();
        }
    }
}