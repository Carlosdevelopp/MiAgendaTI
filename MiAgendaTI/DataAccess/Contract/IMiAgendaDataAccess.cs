using DataAccess.Models.Tables;

namespace DataAccess.Contract;

public interface IMiAgendaDataAccess
{
    #region GET
    Task<Usuario?> GetUserByCredentialAsync(string credential);

    Task<bool> ExistsAsync(string correo, string nombreUsuario);

    Task<List<Contacto>> GetContactoByIdAsync(int id);

    //Task<List<Usuario>> GetAllUsersAsync();
    #endregion

    #region SET
    Task<Usuario> RegisterUserAsync(Usuario usuario);
    #endregion
}
