using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data.SqlClient;
using System.Security.Cryptography;
using tl2_tp10_2023_franciscojvicente.Models;
using tl2_tp10_2023_franciscojvicente.ViewModel;

using System.Text;

namespace tl2_tp10_2023_franciscojvicente.Repository 
{
    public class UserRepository : IUserRepository
    {
        private readonly string _cadenaConexion;

        public UserRepository(string cadenaConexion)
        {
            _cadenaConexion = cadenaConexion;
        }

        public void Create(Usuario usuario)
        {
            var query = "INSERT INTO Usuario (nombre_de_usuario, rol, contrasenia) VALUES (@nombre_de_usuario, @rol, @contrasenia);";
            using (SQLiteConnection connection = new SQLiteConnection(_cadenaConexion))
            {
                connection.Open();
                var command = new SQLiteCommand(query, connection);
                command.Parameters.Add(new SQLiteParameter("@nombre_de_usuario", usuario.NombreDeUsuario));
                command.Parameters.Add(new SQLiteParameter("@rol", usuario.Rol));

                // Hasheamos la contraseña antes de almacenarla en la base de datos usando MD5
                string hashedPassword = CalculateMD5Hash(usuario.Contrasenia);
                command.Parameters.Add(new SQLiteParameter("@contrasenia", hashedPassword));

                var affectedRow = command.ExecuteNonQuery();
                if (affectedRow == 0) 
                {
                    throw new Exception("Se produjo un error al crear el usuario");
                }
                connection.Close();
            }
        }


        public void Delete(int id)
        {
            SQLiteConnection connection = new(_cadenaConexion);
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = $"delete from Usuario where id = @id;";
            command.Parameters.Add(new SQLiteParameter("@id", id));
            connection.Open();
            var affectedRow = command.ExecuteNonQuery();
            if (affectedRow == 0) throw new Exception("Se produjo un error al eliminar el usuario");
            connection.Close();
        }

        public List<UserViewModel> GetAll() {
            var queryString = @"SELECT id, nombre_de_usuario, rol FROM usuario;";
            List<UserViewModel> usuarios = new();
            using (SQLiteConnection connection = new(_cadenaConexion))
            {
                SQLiteCommand command = new(queryString, connection);
                connection.Open();
            
                using(SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var usuario = new UserViewModel();
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
            using (SQLiteConnection connection = new(_cadenaConexion))
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

        public List<UsuarioNameViewModel> GetAllName() {
            var queryString = @"SELECT nombre_de_usuario, id FROM usuario;";
            List<UsuarioNameViewModel> usuarios = new();
            using (SQLiteConnection connection = new(_cadenaConexion))
            {
                SQLiteCommand command = new(queryString, connection);
                connection.Open();
            
                using(SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var usuario = new UsuarioNameViewModel();
                        usuario.Name = reader["nombre_de_usuario"].ToString();
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
            SQLiteConnection connection = new(_cadenaConexion);
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

        public void Update(string username, Roles rol, int id)
        {
            SQLiteConnection connection = new(_cadenaConexion);
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = $"update Usuario set nombre_de_usuario = @nombre_de_usuario, rol = @rol where id = @id;";
            connection.Open();
            command.Parameters.Add(new SQLiteParameter("@id", id));
            command.Parameters.Add(new SQLiteParameter("@nombre_de_usuario", username));
            command.Parameters.Add(new SQLiteParameter("@rol", rol));
            var affectedRow = command.ExecuteNonQuery();
            if (affectedRow == 0) throw new Exception("Se produjo un error al actualizar el usuario");
            connection.Close();
        }

        public void UpdatePass(string pass, int id)
        {
            SQLiteConnection connection = new(_cadenaConexion);
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = $"update Usuario set contrasenia = @contrasenia where id = @id;";
            connection.Open();
            command.Parameters.Add(new SQLiteParameter("@id", id));
            command.Parameters.Add(new SQLiteParameter("@contrasenia", pass));
            string hashedPassword = CalculateMD5Hash(pass);
            command.Parameters.Add(new SQLiteParameter("@contrasenia", hashedPassword));
            var affectedRow = command.ExecuteNonQuery();
            if (affectedRow == 0) throw new Exception("Se produjo un error al cambiar la contraseña del usuario el usuario");
            connection.Close();
        }

            public Usuario Login(string nombre, string contrasenia)
            {
                SQLiteConnection connection = new(_cadenaConexion);
                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = @"SELECT * FROM usuario WHERE nombre_de_usuario = @nombre_de_usuario;";
                command.Parameters.Add(new SQLiteParameter("@nombre_de_usuario", nombre));
                
                connection.Open();
                Usuario? usuario = null;

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        usuario = new Usuario();
                        usuario.Id = Convert.ToInt32(reader["id"]);
                        usuario.NombreDeUsuario = reader["nombre_de_usuario"].ToString();
                        usuario.Rol = (tl2_tp10_2023_franciscojvicente.Models.Roles)(int.TryParse(reader["rol"].ToString(), out var i) ? i : 0);
                        // Obtén el hash almacenado en la base de datos
                        string hashedPasswordFromDatabase = reader["contrasenia"].ToString();
                        // Hashea la contraseña ingresada para compararla con el hash almacenado
                        string hashedPasswordEntered = CalculateMD5Hash(contrasenia);
                        // Compara los hashes
                        if (hashedPasswordFromDatabase.Equals(hashedPasswordEntered, StringComparison.OrdinalIgnoreCase))
                        {
                            // Las contraseñas coinciden, el usuario está autenticado
                            usuario.Contrasenia = hashedPasswordFromDatabase;
                        }
                        else
                        {
                            // Si los hashes no coinciden, establece el usuario en null
                            usuario = null;
                        }
                    }
                }
                connection.Close();
                if (usuario == null) throw new Exception ($"Intento de acceso inválido - Usuario: {nombre} Clave ingresada: {contrasenia}");
                return usuario;
            }


        private string CalculateMD5Hash(string input)
        {
            using MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("x2"));
            }

            return sb.ToString();
        }

    }
}
