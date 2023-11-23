using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data.SqlClient;
using tl2_tp10_2023_franciscojvicente.Models;

namespace tl2_tp10_2023_franciscojvicente.Repository 
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private string cadenaConexion = "Data Source=DB/kanban.db;Cache=Shared";

        public void Create(Usuario usuario)
        {
            var query = $"insert into Usuario (nombre_de_usuario, rol, contrasenia) values (@nombre_de_usuario, @rol, @contrasenia);";
            using SQLiteConnection connection = new(cadenaConexion);
            connection.Open();
            var command = new SQLiteCommand(query, connection);
            command.Parameters.Add(new SQLiteParameter("@nombre_de_usuario", usuario.NombreDeUsuario));
            command.Parameters.Add(new SQLiteParameter("@rol", usuario.Rol));
            command.Parameters.Add(new SQLiteParameter("@contrasenia", usuario.Contrasenia));
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void Delete(int id)
        {
            SQLiteConnection connection = new(cadenaConexion);
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = $"delete from Usuario where id = @id;";
            command.Parameters.Add(new SQLiteParameter("@id", id));
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        public List<Usuario> GetAll() {
            var queryString = @"SELECT * FROM usuario;";
            List<Usuario> usuarios = new();
            using (SQLiteConnection connection = new(cadenaConexion))
            {
                SQLiteCommand command = new(queryString, connection);
                connection.Open();
            
                using(SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var usuario = new Usuario();
                        usuario.Id = Convert.ToInt32(reader["id"]);
                        usuario.NombreDeUsuario = reader["nombre_de_usuario"].ToString();
                        usuario.Rol = (tl2_tp10_2023_franciscojvicente.Models.Roles)(int.TryParse(reader["rol"].ToString(), out var i) ? i : 0);
                        usuario.Contrasenia = reader["contrasenia"].ToString();
                        usuarios.Add(usuario);
                    }
                }
                connection.Close();
            }
            return usuarios;
        }

        public Usuario GetById(int id) {
            SQLiteConnection connection = new(cadenaConexion);
            var usuario = new Usuario();
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM usuario WHERE id = @idUser";
            command.Parameters.Add(new SQLiteParameter("@idUser", id));
            connection.Open();
            using(SQLiteDataReader reader = command.ExecuteReader()) {
                while (reader.Read())
                {
                    usuario.Id = Convert.ToInt32(reader["id"]);
                    usuario.NombreDeUsuario = reader["nombre_de_usuario"].ToString();
                    usuario.Rol = (tl2_tp10_2023_franciscojvicente.Models.Roles)(int.TryParse(reader["rol"].ToString(), out var i) ? i : 0);
                    usuario.Contrasenia = reader["contrasenia"].ToString();
                }
            }
            connection.Close();
            return usuario;     
        }

        public void Update(Usuario usuario, int id)
        {
            SQLiteConnection connection = new(cadenaConexion);
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = $"update Usuario set nombre_de_usuario = @nombre_de_usuario, rol = @rol, contrasenia = @contrasenia where id = @id;";
            command.Parameters.Add(new SQLiteParameter("@id", id));
            command.Parameters.Add(new SQLiteParameter("@nombre_de_usuario", usuario.NombreDeUsuario));
            command.Parameters.Add(new SQLiteParameter("@rol", usuario.Rol));
            command.Parameters.Add(new SQLiteParameter("@contrasenia", usuario.Contrasenia));
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}