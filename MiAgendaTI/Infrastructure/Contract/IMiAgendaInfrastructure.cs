using DataAccess.Models.Tables;

namespace Infrastructure.Contract;

public interface IMiAgendaInfrastructure
{
    #region GET
    Task<List<Usuario>> GetAllUsersAsync();
    #endregion
}
