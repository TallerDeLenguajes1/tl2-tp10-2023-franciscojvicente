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

        public List<UsuarioViewModel> GetAll() {
            var queryString = @"SELECT id, nombre_de_usuario, rol FROM usuario;";
            List<UsuarioViewModel> usuarios = new();
            using (SQLiteConnection connection = new(cadenaConexion))
            {
                SQLiteCommand command = new(queryString, connection);
                connection.Open();
            
                using(SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var usuario = new UsuarioViewModel();
                        usuario.Id = Convert.ToInt32(reader["id"]);
                        usuario.NombreDeUsuario = reader["nombre_de_usuario"].ToString();
                        usuario.Rol = (tl2_tp10_2023_franciscojvicente.Models.Roles)(int.TryParse(reader["rol"].ToString(), out var i) ? i : 0);
                        usuarios.Add(usuario);
                    }
                }
                connection.Close();
            }
            if (usuarios == null) throw new Exception ($"No se encontraron usuarios en la base de datos");
            return usuarios;
        }

        public List<UsuarioIDViewModel> GetAllID() {
            var queryString = @"SELECT id FROM usuario;";
            List<UsuarioIDViewModel> usuarios = new();
            using (SQLiteConnection connection = new(cadenaConexion))
            {
                SQLiteCommand command = new(queryString, connection);
                connection.Open();
            
                using(SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var usuario = new UsuarioIDViewModel();
                        usuario.Id = Convert.ToInt32(reader["id"]);
                        usuarios.Add(usuario);
                    }
                }
                connection.Close();
            }
            if (usuarios == null) throw new Exception ($"No se encontraron usuarios en la base de datos");
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
            if (usuario == null) throw new Exception ($"Error! El usuario no pudo ser encontrado.");
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

        public Usuario Login(string nombre, string contrasenia) {
            SQLiteConnection connection = new(cadenaConexion);
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM usuario where nombre_de_usuario = @nombre_de_usuario and contrasenia = @contrasenia;";
            command.Parameters.Add(new SQLiteParameter("@nombre_de_usuario", nombre));
            command.Parameters.Add(new SQLiteParameter("@contrasenia", contrasenia));
            connection.Open();
            Usuario usuario = null;
            using(SQLiteDataReader reader = command.ExecuteReader()) {
                while (reader.Read())
                {
                    usuario = new Usuario();
                    usuario.Id = Convert.ToInt32(reader["id"]);
                    usuario.NombreDeUsuario = reader["nombre_de_usuario"].ToString();
                    usuario.Rol = (tl2_tp10_2023_franciscojvicente.Models.Roles)(int.TryParse(reader["rol"].ToString(), out var i) ? i : 0);
                    usuario.Contrasenia = reader["contrasenia"].ToString();
                }
            }
                connection.Close();
                if (usuario == null) throw new Exception ($"Intento de acceso inválido - Usuario: {nombre} Clave ingresada: {contrasenia}");
                return usuario;
        }
    }
}
