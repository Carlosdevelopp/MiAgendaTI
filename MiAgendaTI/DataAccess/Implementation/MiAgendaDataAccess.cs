using DataAccess.Contract;
using DataAccess.Models.Tables;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DataAccess
{
    public class MiAgendaDataAccess : IMiAgendaDataAccess
    {
        private readonly SqlConnection _dbConnection;

        public MiAgendaDataAccess(SqlConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        #region GET
        //Get List Users
        public async Task<List<Usuarios_ET>> GetUsuariosList()
        {
            var usuarios = new List<Usuarios_ET>();

            using var command = _dbConnection.CreateCommand();
            command.CommandText = "sp_GetAllUsers";
            command.CommandType = CommandType.StoredProcedure;

            if (_dbConnection.State != ConnectionState.Open)
                await _dbConnection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                usuarios.Add(new Usuarios_ET
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
}
