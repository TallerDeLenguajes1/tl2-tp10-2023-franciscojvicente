using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tl2_tp10_2023_franciscojvicente.Models;

namespace tl2_tp10_2023_franciscojvicente.Repository  {
    public interface IUsuarioRepository
    {
        public List<Usuario> GetAll();
        public Usuario GetById(int id);
        public void Create(Usuario usuario);
        public void Update(Usuario usuario, int id);
        public void Delete(int id);

    }
}