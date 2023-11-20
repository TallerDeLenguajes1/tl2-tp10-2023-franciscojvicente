using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data.SqlClient;
using tl2_tp10_2023_franciscojvicente.Models;

namespace tl2_tp10_2023_franciscojvicente.Repository  {
    public class TableroRepository : ITableroRepository
    {
        private string cadenaConexion = "Data Source=DB/kanban.db;Cache=Shared";
        public void Create(Tablero tablero)
        {
            var query = $"insert into Tablero (id_usuario_propietario, nombre, descripcion) values (@id_usuario_propietario, @nombre, @descripcion);";
            using SQLiteConnection connection = new(cadenaConexion);
            connection.Open();
            var command = new SQLiteCommand(query, connection);
            command.Parameters.Add(new SQLiteParameter("@id_usuario_propietario", tablero.Id_usuario_propietario));
            command.Parameters.Add(new SQLiteParameter("@nombre", tablero.Nombre));
            command.Parameters.Add(new SQLiteParameter("@descripcion", tablero.Descripcion));
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void Delete(int idTablero)
        {
            SQLiteConnection connection = new(cadenaConexion);
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = $"delete from Tablero where id = @id;";
            command.Parameters.Add(new SQLiteParameter("@id", idTablero));
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        public List<Tablero> GetAll()
        {
            var queryString = @"SELECT * FROM tablero;";
            List<Tablero> tableros = new();
            using (SQLiteConnection connection = new(cadenaConexion))
            {
                SQLiteCommand command = new(queryString, connection);
                connection.Open();
            
                using(SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var tablero = new Tablero
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Id_usuario_propietario = Convert.ToInt32(reader["id_usuario_propietario"]),
                            Nombre = reader["nombre"].ToString(),
                            Descripcion = reader["descripcion"].ToString()
                        };
                        tableros.Add(tablero);
                    }
                }
                connection.Close();
            }
            return tableros;
        }

        public List<Tablero> GetAllByUser(int idUser)
        {
            SQLiteConnection connection = new(cadenaConexion);
            List<Tablero> tableros = new();
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "select * from Tablero where (Tablero.id_usuario_propietario = @idUser);";
            command.Parameters.Add(new SQLiteParameter("@idUser", idUser));
            connection.Open();
            using (SQLiteDataReader reader = command.ExecuteReader()) {
                while (reader.Read()) {
                    var tablero = new Tablero
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Id_usuario_propietario = Convert.ToInt32(reader["id_usuario_propietario"]),
                            Nombre = reader["nombre"].ToString(),
                            Descripcion = reader["descripcion"].ToString()
                        };
                        tableros.Add(tablero);
                }
            }
            connection.Close();
            return tableros;
        }

        public Tablero GetById(int idTablero)
        {
            SQLiteConnection connection = new(cadenaConexion);
            var tablero = new Tablero();
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM tablero WHERE id = @idTablero";
            command.Parameters.Add(new SQLiteParameter("@idTablero", idTablero));
            connection.Open();
            using(SQLiteDataReader reader = command.ExecuteReader()) {
                while (reader.Read())
                {
                    tablero.Id = Convert.ToInt32(reader["id"]);
                    tablero.Id_usuario_propietario = Convert.ToInt32(reader["id_usuario_propietario"]);
                    tablero.Nombre = reader["nombre"].ToString();
                    tablero.Descripcion = reader["descripcion"].ToString();
                }
            }
            connection.Close();
            return tablero;
        }

        public void Update(Tablero tablero, int idTablero)
        {
            SQLiteConnection connection = new(cadenaConexion);
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = $"update Tablero set id_usuario_propietario = @idUser, nombre = @nombre, descripcion = @descripcion where id = @idTablero;";
            command.Parameters.Add(new SQLiteParameter("@idTablero", idTablero));
            command.Parameters.Add(new SQLiteParameter("@idUser", tablero.Id_usuario_propietario));
            command.Parameters.Add(new SQLiteParameter("@nombre", tablero.Nombre));
            command.Parameters.Add(new SQLiteParameter("@descripcion", tablero.Descripcion));
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close(); 
        }
    }
}