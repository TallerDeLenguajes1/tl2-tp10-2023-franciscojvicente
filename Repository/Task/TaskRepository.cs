using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data.SqlClient;
using tl2_tp10_2023_franciscojvicente.Models;
using tl2_tp10_2023_franciscojvicente.ViewModel;

namespace tl2_tp10_2023_franciscojvicente.Repository 
{
    public class TaskRepository : ITaskRepository
    {
        private readonly string _cadenaConexion;

        public TaskRepository(string cadenaConexion)
        {
            _cadenaConexion = cadenaConexion;
        }
        public void Create(Tarea tarea)
        {
            var query = $"insert into Tarea (id_tablero, nombre, estado, descripcion, color, id_usuario_asignado) values (@id_tablero, @nombre, @estado, @descripcion, @color, @id_usuario_asignado);";
            using SQLiteConnection connection = new(_cadenaConexion);
            connection.Open();
            var command = new SQLiteCommand(query, connection);
            command.Parameters.Add(new SQLiteParameter("@id_tablero", tarea.IdTablero));
            command.Parameters.Add(new SQLiteParameter("@nombre", tarea.Nombre));
            command.Parameters.Add(new SQLiteParameter("@estado", (int)tarea.EstadoTarea));
            command.Parameters.Add(new SQLiteParameter("@descripcion", tarea.Descripcion));
            command.Parameters.Add(new SQLiteParameter("@color", tarea.Color));
            command.Parameters.Add(new SQLiteParameter("@id_usuario_asignado", tarea.Id_usuario_asignado));
            var affectedRow = command.ExecuteNonQuery();
            if (affectedRow == 0) throw new Exception("Se produjo un error al crear la tarea");
            connection.Close();
        }

        public void Delete(int idTarea)
        {
            SQLiteConnection connection = new(_cadenaConexion);
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = $"delete from Tarea where id = @id;";
            command.Parameters.Add(new SQLiteParameter("@id", idTarea));
            connection.Open();
            var affectedRow = command.ExecuteNonQuery();
            if (affectedRow == 0) throw new Exception("Se produjo un error al eliminar la tarea");
            connection.Close();
        }

        public void DeleteByBoard(int idTablero)
        {
            SQLiteConnection connection = new(_cadenaConexion);
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = $"delete from Tarea where id_tablero = @id;";
            command.Parameters.Add(new SQLiteParameter("@id", idTablero));
            connection.Open();
            var affectedRow = command.ExecuteNonQuery();
            if (affectedRow < 0) throw new Exception($"Se produjo un error al eliminar las tareas del tablero {idTablero}");
            connection.Close();
        }

        public void DeleteByUser(int idUser)
        {
            SQLiteConnection connection = new(_cadenaConexion);
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = $"delete from Tarea where id_usuario_asignado = @id;";
            command.Parameters.Add(new SQLiteParameter("@id", idUser));
            connection.Open();
            var affectedRow = command.ExecuteNonQuery();
            if (affectedRow < 0) throw new Exception($"Se produjo un error al eliminar las tareas del usuario {idUser}");
            connection.Close();
        }

        public List<TaskViewModel> GetAllTareasByTablero(int idTablero)
        {
            SQLiteConnection connection = new(_cadenaConexion);
            List<TaskViewModel> tareas = new();
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT T.id, T.id_tablero, T.nombre, T.estado, T.descripcion, T.color, T.id_usuario_asignado, U.nombre_de_usuario, TB.nombre AS nombre_tablero FROM TAREA T LEFT JOIN USUARIO U ON T.id_usuario_asignado = U.id INNER JOIN TABLERO TB ON T.id_tablero = TB.id WHERE T.id_tablero = @idTablero;";
            command.Parameters.Add(new SQLiteParameter("@idTablero", idTablero));
            connection.Open();
            using (SQLiteDataReader reader = command.ExecuteReader()) {
                while (reader.Read()) {
                    TaskViewModel tarea = new()
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        IdTablero = Convert.ToInt32(reader["id_tablero"]),
                        Nombre = reader["nombre"].ToString(),
                        EstadoTarea = (tl2_tp10_2023_franciscojvicente.Models.StatusTask)(int.TryParse(reader["estado"].ToString(), out var i)? i: 0),
                        Descripcion = reader["descripcion"].ToString(),
                        Color = reader["color"].ToString(),
                        Id_usuario_asignado = Convert.ToInt32(reader["id_usuario_asignado"]),
                        NombreProp = reader["nombre_de_usuario"].ToString(),
                        OwnerBoard = reader["nombre_tablero"].ToString(),
                    };
                    tareas.Add(tarea);
                }
            }
            connection.Close();
            if (tareas == null) throw new Exception ($"No se encontraron tareas en la base de datos");
            return tareas;
        }

        public List<TaskViewModel> GetAllTareasByUser(int idUser)
        {
            SQLiteConnection connection = new(_cadenaConexion);
            List<TaskViewModel> tareas = new();
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT T.id, T.id_tablero, T.nombre, T.estado, T.descripcion, T.color, T.id_usuario_asignado, U.nombre_de_usuario, TB.nombre AS nombre_tablero FROM TAREA T LEFT JOIN USUARIO U ON T.id_usuario_asignado = U.id INNER JOIN TABLERO TB ON T.id_tablero = TB.id WHERE T.id_usuario_asignado = @idUser;";
            command.Parameters.Add(new SQLiteParameter("@idUser", idUser));
            connection.Open();
            using (SQLiteDataReader reader = command.ExecuteReader()) {
                while (reader.Read()) {
                    var tarea = new TaskViewModel();
                        tarea.Id = Convert.ToInt32(reader["id"]);
                        tarea.IdTablero = Convert.ToInt32(reader["id_tablero"]);
                        tarea.Nombre = reader["nombre"].ToString();
                        tarea.EstadoTarea = (tl2_tp10_2023_franciscojvicente.Models.StatusTask)(int.TryParse(reader["estado"].ToString(), out var i) ? i : 0);
                        tarea.Descripcion = reader["descripcion"].ToString();
                        tarea.Color = reader["color"].ToString();
                        tarea.Id_usuario_asignado = Convert.ToInt32(reader["id_usuario_asignado"]);
                        tarea.NombreProp = reader["nombre_de_usuario"].ToString();
                        tarea.OwnerBoard = reader["nombre_tablero"].ToString();
                    tareas.Add(tarea);
                }
            }
            connection.Close();
            if (tareas == null) throw new Exception ($"No se encontraron tareas asignadas al usuario {idUser} en la base de datos");
            return tareas;
        }

        public Tarea GetById(int idTarea)
        {
            SQLiteConnection connection = new(_cadenaConexion);
            var tarea = new Tarea();
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Tarea WHERE id = @idTarea";
            command.Parameters.Add(new SQLiteParameter("@idTarea", idTarea));
            connection.Open();
            using(SQLiteDataReader reader = command.ExecuteReader()) {
                while (reader.Read())
                {
                    tarea.Id = Convert.ToInt32(reader["id"]);
                    tarea.IdTablero = Convert.ToInt32(reader["id_tablero"]);
                    tarea.Nombre = reader["nombre"].ToString();
                    tarea.EstadoTarea = (tl2_tp10_2023_franciscojvicente.Models.StatusTask)(int.TryParse(reader["estado"].ToString(), out var i)? i: 0);
                    tarea.Descripcion = reader["descripcion"].ToString();
                    tarea.Color = reader["color"].ToString();
                    tarea.Id_usuario_asignado = Convert.ToInt32(reader["id_usuario_asignado"]);
                }
            }
            connection.Close();
            if (tarea == null) throw new Exception ($"No se encontró dicha tarea en la base de datos");
            return tarea;
        }

        public void Update(Tarea tarea, int idTarea)
        {
            SQLiteConnection connection = new(_cadenaConexion);
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText =$"update Tarea set id_tablero = @idTablero, nombre = @nombre, estado = @estado, descripcion = @descripcion, color = @color, id_usuario_asignado = @id_usuario_asignado where id = @idTarea;";
            command.Parameters.Add(new SQLiteParameter("@idTarea", idTarea));
            command.Parameters.Add(new SQLiteParameter("@idTablero", tarea.IdTablero));
            command.Parameters.Add(new SQLiteParameter("@nombre", tarea.Nombre));
            command.Parameters.Add(new SQLiteParameter("@estado", (int)tarea.EstadoTarea));
            command.Parameters.Add(new SQLiteParameter("@descripcion", tarea.Descripcion));
            command.Parameters.Add(new SQLiteParameter("@color", tarea.Color));
            command.Parameters.Add(new SQLiteParameter("@id_usuario_asignado", tarea.Id_usuario_asignado));
            connection.Open();
            var affectedRow = command.ExecuteNonQuery();
            if (affectedRow == 0) throw new Exception("Se produjo un error al actualizar la tarea");
            connection.Close();            
        }

        public void UpdateStatus(int idTask, int status)
        {
            SQLiteConnection connection = new(_cadenaConexion);
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = $"update tarea set estado = @estado where id = @idTarea;";
            command.Parameters.Add(new SQLiteParameter("@estado", status));
            command.Parameters.Add(new SQLiteParameter("@idTarea", idTask));
            connection.Open();
            var affectedRow = command.ExecuteNonQuery();
            if (affectedRow == 0) throw new Exception("Se produjo un error al actualizar la tarea");
            connection.Close();  
        }

        public List<TaskViewModel> GetAll()
        {
            var queryString = "SELECT T.id, T.nombre, T.estado, T.descripcion, T.color, U.nombre_de_usuario AS nombre_usuario_asignado, TB.nombre AS nombre_tablero FROM TAREA T LEFT JOIN USUARIO U ON T.id_usuario_asignado = U.id INNER JOIN TABLERO TB ON T.id_tablero = TB.id;";
            List<TaskViewModel> tareas = new List<TaskViewModel>();
            using (SQLiteConnection connection = new SQLiteConnection(_cadenaConexion))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(queryString, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var tarea = new TaskViewModel();
                            tarea.Id = Convert.ToInt32(reader["id"]);
                            tarea.IdTablero = Convert.ToInt32(reader["id_tablero"]);
                            tarea.Nombre = reader["nombre"].ToString();
                            tarea.EstadoTarea = (tl2_tp10_2023_franciscojvicente.Models.StatusTask)(int.TryParse(reader["estado"].ToString(), out var i) ? i : 0);
                            tarea.Descripcion = reader["descripcion"].ToString();
                            tarea.Color = reader["color"].ToString();
                            tarea.Id_usuario_asignado = Convert.ToInt32(reader["id_usuario_asignado"]);
                            tarea.NombreProp = reader["nombre_de_usuario"].ToString();
                            tarea.OwnerBoard = reader["nombre"].ToString();
                            tareas.Add(tarea);
                        }
                    }
                }
            }
            if (tareas == null) throw new Exception ($"No se encontraron tareas en la base de datos");
            return tareas;
        }
    }
}