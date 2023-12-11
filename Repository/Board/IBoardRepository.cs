using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tl2_tp10_2023_franciscojvicente.Models;
using tl2_tp10_2023_franciscojvicente.ViewModel;

namespace tl2_tp10_2023_franciscojvicente.Repository {
    public interface IBoardRepository
    {
        public Tablero GetById(int idTablero);
        public void Create(Tablero tablero);
        public void Update(Tablero tablero, int idTablero);
        public List<Tablero> GetAll();
        public List<Tablero> GetAllByUser(int idUser);
        public void Delete(int idTablero);
        public void DeleteByUser(int idUser);
        public List<TableroIDViewModel> GetAllID();
        public List<Tablero> GetAllOwnAndAssigned(int idUser);
        public List<TableroIDViewModel> GetAllIDByUser(int idUser);
        
    }
}
