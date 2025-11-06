using DataAccess.Models.Tables;

namespace DataAccess.Contract;

public interface IMiAgendaDataAccess
{
    #region GET
    Task<List<Usuario>> GetAllUsersAsync();
    #endregion
}
