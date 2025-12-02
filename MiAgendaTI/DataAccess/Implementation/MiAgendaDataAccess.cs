using DataAccess.Contract;
using DataAccess.Models.Tables;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

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
}
