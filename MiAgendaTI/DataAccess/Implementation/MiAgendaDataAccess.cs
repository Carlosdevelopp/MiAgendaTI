using DataAccess.Contract;
using DataAccess.Models.Tables;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace DataAccess;

public class MiAgendaDataAccess : IMiAgendaDataAccess
{
    private readonly string _connectionString;

    public MiAgendaDataAccess(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("AGENDA_DB_CONNECTION")!;
    }

    #region GET
    public async Task<Usuario?> GetUserByCredentialAsync(string credential)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        using var command = new SqlCommand("GetUserByCredential", connection);
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@Cedential", credential);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Usuario
            {
                UsuarioId = reader.GetInt32(reader.GetOrdinal("UsuarioId")),
                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                PrimerApellido = reader.GetString(reader.GetOrdinal("PrimerApellido")),
                SegundoApellido = reader.GetString(reader.GetOrdinal("SegundoApellido")),
                Correo = reader.GetString(reader.GetOrdinal("Correo")),
                NombreUsuario = reader.GetString(reader.GetOrdinal("NombreUsuario")),
                Telefono = reader.GetString(reader.GetOrdinal("Telefono")),
                Estado = reader.GetBoolean(reader.GetOrdinal("Estado"))
            };
        }
        return null;
    }

    public async Task<bool> ExistsAsync(string correo, string nombreUsuario)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = new SqlCommand("UserExists", connection);
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@Correo", correo);
        command.Parameters.AddWithValue("@NombreUsuario", nombreUsuario);

        int result = Convert.ToInt32(await command.ExecuteScalarAsync());
        return result == 1;
    }

    public async Task<List<Contacto>> GetContactoByIdAsync(int id)
    {
        var contactosDict = new Dictionary<int, Contacto>();

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = new SqlCommand("GetContactById", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add("@ContactId", SqlDbType.Int).Value = id;

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            int contactoId = reader.GetInt32(reader.GetOrdinal("ContactoId"));

            if (!contactosDict.ContainsKey(contactoId))
            {
                var contacto = new Contacto
                {
                    ContactoId = reader.GetInt32(reader.GetOrdinal("ContactoId")),
                    UsuarioId = reader.GetInt32(reader.GetOrdinal("UsuarioId")),
                    Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                    PrimerApellido = reader.GetString(reader.GetOrdinal("PrimerApellido")),
                    SegundoApellido = reader.GetString(reader.GetOrdinal("SegundoApellido")),
                    FechaNacimiento = reader.GetDateTime(reader.GetOrdinal("FechaNacimiento")),
                    FotoRuta = reader.GetString(reader.GetOrdinal("FotoRuta")),
                    FechaRegistro = reader.GetDateTime(reader.GetOrdinal("FechaRegistro")),
                    Telefono = reader.GetString(reader.GetOrdinal("Telefono"))
                };
                contactosDict.Add(contactoId, contacto);
            }

            if (!reader.IsDBNull(reader.GetOrdinal("DetContactoRedId")))
            {
                //Agrega cada detalle a  su Contacto (1:N)
                contactosDict[contactoId].DetalleContacto.Add(new DetalleContacto
                {

                    DetContactoRedId = reader.GetInt32(reader.GetOrdinal("DetContactoRedId")),
                    ContactoId = reader.GetInt32(reader.GetOrdinal("ContactoId")),
                    TipoContactoId = reader.GetInt32(reader.GetOrdinal("TipoContactoId")),
                    URL = reader.GetString(reader.GetOrdinal("URL")),
                    NombreUsuarioRed = reader.GetString(reader.GetOrdinal("NombreUsuarioRed")),
                    FechaRegistro = reader.GetDateTime(reader.GetOrdinal("FechaRegistro"))
                });
            }
        }
        return contactosDict.Values.ToList();
    }   

    //public async Task<List<Usuario>> GetAllUsersAsync()
    //{
    //    var usuarios = new List<Usuario>();

    //    using var connection = new SqlConnection(_connectionString);
    //    await connection.OpenAsync();

    //    using var command = new SqlCommand("GetAllUsers", connection);
    //    command.CommandType = CommandType.StoredProcedure;

    //    using var reader = await command.ExecuteReaderAsync();
    //    while (await reader.ReadAsync())
    //    {
    //        usuarios.Add(new Usuario
    //        {
    //            UsuarioId = reader.GetInt32(reader.GetOrdinal("UsuarioId")),
    //            Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
    //            PrimerApellido = reader.GetString(reader.GetOrdinal("PrimerApellido")),
    //            SegundoApellido = reader.GetString(reader.GetOrdinal("SegundoApellido")),
    //            Correo = reader.GetString(reader.GetOrdinal("Correo")),
    //            NombreUsuario = reader.GetString(reader.GetOrdinal("NombreUsuario")),
    //            Telefono = reader.GetString(reader.GetOrdinal("Telefono")),
    //            Estado = reader.GetBoolean(reader.GetOrdinal("Estado"))
    //        });
    //    }
    //    return usuarios;
    //}
    #endregion

    #region SET
    public async Task<Usuario> RegisterUserAsync(Usuario usuario)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = new SqlCommand("RegisterUser", connection);
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.Add("@Nombre", SqlDbType.NVarChar, 50).Value = usuario.Nombre;
        command.Parameters.Add("@PrimerApellido", SqlDbType.NVarChar, 50).Value = usuario.PrimerApellido;
        command.Parameters.Add("@SegundoApellido", SqlDbType.NVarChar, 50).Value = usuario.SegundoApellido ?? (object)DBNull.Value;
        command.Parameters.Add("@Correo", SqlDbType.NVarChar, 50).Value = usuario.Correo;
        command.Parameters.Add("@NombreUsuario", SqlDbType.NVarChar, 50).Value = usuario.NombreUsuario;
        command.Parameters.Add("@PasswordHash", SqlDbType.NVarChar, 50).Value = usuario.Password;
        command.Parameters.Add("@URLFoto", SqlDbType.NVarChar, 50).Value = usuario.RutaFoto ?? (object)DBNull.Value;
        command.Parameters.Add("@Telefono", SqlDbType.NVarChar, 50).Value = usuario.Telefono ?? (object)DBNull.Value;
        command.Parameters.Add("@FechaRegistro", SqlDbType.DateTime2).Value = usuario.FechaRegistro;
        command.Parameters.Add("@Estado", SqlDbType.Bit).Value = usuario.Estado;

        var result = await command.ExecuteScalarAsync();
        int id = Convert.ToInt32(result);

        if (id == -1)
            throw new Exception("El usuario ya existe.");

        usuario.UsuarioId = id;
        return usuario;
    }
    #endregion
}
