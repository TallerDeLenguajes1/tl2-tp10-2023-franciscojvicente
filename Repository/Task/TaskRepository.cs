using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data.SqlClient;
using tl2_tp10_2023_franciscojvicente.Models;

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

        public List<Tarea> GetAllTareasByTablero(int idTablero)
        {
            SQLiteConnection connection = new(_cadenaConexion);
            List<Tarea> tareas = new();
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "select * from Tarea where (Tarea.id_tablero = @idTablero);";
            command.Parameters.Add(new SQLiteParameter("@idTablero", idTablero));
            connection.Open();
            using (SQLiteDataReader reader = command.ExecuteReader()) {
                while (reader.Read()) {
                    Tarea tarea = new()
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        IdTablero = Convert.ToInt32(reader["id_tablero"]),
                        Nombre = reader["nombre"].ToString(),
                        EstadoTarea = (tl2_tp10_2023_franciscojvicente.Models.StatusTask)(int.TryParse(reader["estado"].ToString(), out var i)? i: 0),
                        Descripcion = reader["descripcion"].ToString(),
                        Color = reader["color"].ToString(),
                        Id_usuario_asignado = Convert.ToInt32(reader["id_usuario_asignado"])
                    };
                    tareas.Add(tarea);
                }
            }
            connection.Close();
            if (tareas == null) throw new Exception ($"No se encontraron tareas en la base de datos");
            return tareas;
        }

        public List<Tarea> GetAllTareasByUser(int idUser)
        {
            SQLiteConnection connection = new(_cadenaConexion);
            List<Tarea> tareas = new();
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "select * from Tarea where (Tarea.id_usuario_asignado = @idUser);";
            command.Parameters.Add(new SQLiteParameter("@idUser", idUser));
            connection.Open();
            using (SQLiteDataReader reader = command.ExecuteReader()) {
                while (reader.Read()) {
                    Tarea tarea = new()
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        IdTablero = Convert.ToInt32(reader["id_tablero"]),
                        Nombre = reader["nombre"].ToString(),
                        EstadoTarea = (tl2_tp10_2023_franciscojvicente.Models.StatusTask)(int.TryParse(reader["estado"].ToString(), out var i)? i: 0),
                        Descripcion = reader["descripcion"].ToString(),
                        Color = reader["color"].ToString(),
                        Id_usuario_asignado = Convert.ToInt32(reader["id_usuario_asignado"])
                    };
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

        public List<Tarea> GetAll()
        {
            var queryString = "SELECT * FROM Tarea;";
            List<Tarea> tareas = new List<Tarea>();
            using (SQLiteConnection connection = new SQLiteConnection(_cadenaConexion))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(queryString, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var tarea = new Tarea();
                            tarea.Id = Convert.ToInt32(reader["id"]);
                            tarea.IdTablero = Convert.ToInt32(reader["id_tablero"]);
                            tarea.Nombre = reader["nombre"].ToString();
                            tarea.EstadoTarea = (tl2_tp10_2023_franciscojvicente.Models.StatusTask)(int.TryParse(reader["estado"].ToString(), out var i) ? i : 0);
                            tarea.Descripcion = reader["descripcion"].ToString();
                            tarea.Color = reader["color"].ToString();
                            tarea.Id_usuario_asignado = Convert.ToInt32(reader["id_usuario_asignado"]);
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