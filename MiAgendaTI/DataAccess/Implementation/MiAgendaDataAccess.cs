using DataAccess.Contract;
using DataAccess.Models.Tables;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Runtime.CompilerServices;

namespace DataAccess;

public class MiAgendaDataAccess : IMiAgendaDataAccess
{
    private readonly string _connectionStrings;

    public MiAgendaDataAccess(IConfiguration configuration)
    {
        _connectionStrings = configuration.GetConnectionString("AGENDA_DB_CONNECTION")!;
    }

    #region GET
    public async Task<Usuario?> GetUserByCredentialAsync(string credential)
    {
        using var connection = new SqlConnection(_connectionStrings);
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

    public async Task<bool> ExistsAsync(string SearchValue)
    {
        using var connection = new SqlConnection(_connectionStrings);
        await connection.OpenAsync();

        using var command = new SqlCommand("UserExists", connection);
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@Valor", SearchValue);

        int result = Convert.ToInt32(await command.ExecuteScalarAsync());
        return result == 1;
    }

    public async Task<List<Usuario>> GetAllUsersAsync()
    {
        var usuarios = new List<Usuario>();

        using var connection = new SqlConnection(_connectionStrings);
        await connection.OpenAsync();

        using var command = new SqlCommand("GetAllUsers", connection);
        command.CommandType = CommandType.StoredProcedure;

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            usuarios.Add(new Usuario
            {
                UsuarioId = reader.GetInt32(reader.GetOrdinal("UsuarioId")),
                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                PrimerApellido = reader.GetString(reader.GetOrdinal("PrimerApellido")),
                SegundoApellido = reader.GetString(reader.GetOrdinal("SegundoApellido")),
                Correo = reader.GetString(reader.GetOrdinal("Correo")),
                NombreUsuario = reader.GetString(reader.GetOrdinal("NombreUsuario")),
                Telefono = reader.GetString(reader.GetOrdinal("Telefono")),
                Estado = reader.GetBoolean(reader.GetOrdinal("Estado"))
            });
        }
        return usuarios;
    }
    #endregion

    #region SET
    public async Task<Usuario> RegisterUser(Usuario usuario)
    {
        await using var connection = new SqlConnection(_connectionStrings);
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
