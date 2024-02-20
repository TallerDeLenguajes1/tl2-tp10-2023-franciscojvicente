using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tl2_tp10_2023_franciscojvicente.Models;
using tl2_tp10_2023_franciscojvicente.ViewModel;

namespace tl2_tp10_2023_franciscojvicente.Repository  {
    public interface ITaskRepository
    {
        public List<TaskViewModel> GetAll();
        public List<TaskViewModel> GetAllTareasByUser(int idUser);
        public List<TaskViewModel> GetAllTareasByTablero(int idTablero);
        public Tarea GetById(int idTarea);
        public void Create(Tarea tarea);
        public void Update(Tarea tarea, int idTarea);
        public void Delete(int idTarea);
        public void UpdateStatus(int idTask, int status);
        public void DeleteByBoard(int idTablero);
        public void DeleteByUser(int idUser);
    }
}