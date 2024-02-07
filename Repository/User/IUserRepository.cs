using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tl2_tp10_2023_franciscojvicente.Models;
using tl2_tp10_2023_franciscojvicente.ViewModel;
namespace tl2_tp10_2023_franciscojvicente.Repository  {
    public interface IUserRepository
    {
        public List<UserViewModel
        > GetAll();
        public Usuario GetById(int id);
        public void Create(Usuario usuario);
        public void Update(Usuario usuario, int id);
        public void UpdatePass(string pass, int id);
        public void Delete(int id);
        public Usuario Login(string nombre, string contrasenia);
        public List<UsuarioIDViewModel> GetAllID();
    }
}