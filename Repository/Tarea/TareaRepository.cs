using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data.SqlClient;
using tl2_tp10_2023_franciscojvicente.Models;

namespace tl2_tp10_2023_franciscojvicente.Repository 
{
    public class TareaRepository : ITareaRepository
    {
        private string cadenaConexion = "Data Source=DB/kanban.db;Cache=Shared";
        public void AddUserToTarea(int idUser, int idTarea)
        {
            SQLiteConnection connection = new(cadenaConexion);
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = $"update Tarea set id_usuario_asignado = @idUser where id = @idTarea;";
            command.Parameters.Add(new SQLiteParameter("@idUser", idUser));
            command.Parameters.Add(new SQLiteParameter("@idTarea", idTarea));
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void Create(Tarea tarea)
        {
            var query = $"insert into Tarea (id_tablero, nombre, estado, descripcion, color, id_usuario_asignado) values (@id_tablero, @nombre, @estado, @descripcion, @color, @id_usuario_asignado);";
            using SQLiteConnection connection = new(cadenaConexion);
            connection.Open();
            var command = new SQLiteCommand(query, connection);
            command.Parameters.Add(new SQLiteParameter("@id_tablero", tarea.IdTablero));
            command.Parameters.Add(new SQLiteParameter("@nombre", tarea.Nombre));
            command.Parameters.Add(new SQLiteParameter("@estado", (int)tarea.EstadoTarea)); // ToDo: Castear hasta que llegue correctamente
            command.Parameters.Add(new SQLiteParameter("@descripcion", tarea.Descripcion));
            command.Parameters.Add(new SQLiteParameter("@color", tarea.Color));
            command.Parameters.Add(new SQLiteParameter("@id_usuario_asignado", tarea.Id_usuario_asignado));
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void Delete(int idTarea)
        {
            SQLiteConnection connection = new(cadenaConexion);
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = $"delete from Tarea where id = @id;";
            command.Parameters.Add(new SQLiteParameter("@id", idTarea));
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        public List<Tarea> GetAllTareasByTablero(int idTablero)
        {
            SQLiteConnection connection = new(cadenaConexion);
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
                        EstadoTarea = (tl2_tp10_2023_franciscojvicente.Models.EstadoTarea)(int.TryParse(reader["estado"].ToString(), out var i)? i: 0),
                        Descripcion = reader["descripcion"].ToString(),
                        Color = reader["color"].ToString(),
                        Id_usuario_asignado = Convert.ToInt32(reader["id_usuario_asignado"])
                    };
                    tareas.Add(tarea);
                }
            }
            connection.Close();
            return tareas;
        }

        public List<Tarea> GetAllTareasByUser(int idUser)
        {
            SQLiteConnection connection = new(cadenaConexion);
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
                        EstadoTarea = (tl2_tp10_2023_franciscojvicente.Models.EstadoTarea)(int.TryParse(reader["estado"].ToString(), out var i)? i: 0),
                        Descripcion = reader["descripcion"].ToString(),
                        Color = reader["color"].ToString(),
                        Id_usuario_asignado = Convert.ToInt32(reader["id_usuario_asignado"])
                    };
                    tareas.Add(tarea);
                }
            }
            connection.Close();
            return tareas;
        }

        public Tarea GetById(int idTarea)
        {
            SQLiteConnection connection = new(cadenaConexion);
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
                    tarea.EstadoTarea = (tl2_tp10_2023_franciscojvicente.Models.EstadoTarea)(int.TryParse(reader["estado"].ToString(), out var i)? i: 0);
                    tarea.Descripcion = reader["descripcion"].ToString();
                    tarea.Color = reader["color"].ToString();
                    tarea.Id_usuario_asignado = Convert.ToInt32(reader["id_usuario_asignado"]);
                }
            }
            connection.Close();
            return tarea;
        }

        public void Update(Tarea tarea, int idTarea)
        {
            //if(Convert.ToInt32(tarea.EstadoTarea) > 4) return;
            SQLiteConnection connection = new(cadenaConexion);
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
            command.ExecuteNonQuery();
            connection.Close();            
        }
        
        public List<Tarea>? GetTareasPorEstado(int estado)
        {
            if(estado > 4) return null;
            SQLiteConnection connection = new(cadenaConexion);
            List<Tarea> tareas = new();
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = $"select * from tarea where estado = @estado;";
            command.Parameters.Add(new SQLiteParameter("@estado", estado));
            connection.Open();
            using (SQLiteDataReader reader = command.ExecuteReader()) {
                while (reader.Read()) {
                    Tarea tarea = new()
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        IdTablero = Convert.ToInt32(reader["id_tablero"]),
                        Nombre = reader["nombre"].ToString(),
                        EstadoTarea = (tl2_tp10_2023_franciscojvicente.Models.EstadoTarea)(int.TryParse(reader["estado"].ToString(), out var i)? i: 0),
                        Descripcion = reader["descripcion"].ToString(),
                        Color = reader["color"].ToString(),
                        Id_usuario_asignado = Convert.ToInt32(reader["id_usuario_asignado"])
                    };
                    tareas.Add(tarea);
                }
            }
            connection.Close();
            return tareas;
        }

        public void UpdateEstado(int idTarea, int estado)
        {
            if(estado > 4) return;
            SQLiteConnection connection = new(cadenaConexion);
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = $"update tarea set estado = @estado where id = @idTarea;";
            command.Parameters.Add(new SQLiteParameter("@estado", estado));
            command.Parameters.Add(new SQLiteParameter("@idTarea", idTarea));
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();            
        }

        public List<Tarea> GetAll()
        {
            var queryString = "SELECT * FROM Tarea;";
            List<Tarea> tareas = new List<Tarea>();

            using (SQLiteConnection connection = new SQLiteConnection(cadenaConexion))
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
                            tarea.EstadoTarea = (tl2_tp10_2023_franciscojvicente.Models.EstadoTarea)(int.TryParse(reader["estado"].ToString(), out var i) ? i : 0);
                            tarea.Descripcion = reader["descripcion"].ToString();
                            tarea.Color = reader["color"].ToString();
                            tarea.Id_usuario_asignado = Convert.ToInt32(reader["id_usuario_asignado"]);
                            tareas.Add(tarea);
                        }
                    }
                }
            }

            return tareas;
        }

    }
}