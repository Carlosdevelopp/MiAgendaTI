using DataAccess.Models.Tables;

namespace DataAccess.Contract;

public interface IMiAgendaDataAccess
{
    #region GET
    Task<Usuario?> GetUserByCredentialAsync(string credential);

    Task<bool> ExistsAsync(string SearchValue);

    Task<List<Contacto>> GetContactoById(int id);

    Task<List<Usuario>> GetAllUsersAsync();
    #endregion

    #region SET
    Task<Usuario> RegisterUserAsync(Usuario usuario);
    #endregion
}
