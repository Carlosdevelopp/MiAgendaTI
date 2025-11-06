using DataAccess.Contract;
using DataAccess.Models.Tables;
using Infrastructure.Contract;

namespace Infrastructure.Implementation;

public class MiAgendaInfrastructure : IMiAgendaInfrastructure
{
    private readonly IMiAgendaDataAccess _agendaDA;

    public MiAgendaInfrastructure(IMiAgendaDataAccess agendaDataAccess)
    {
        _agendaDA = agendaDataAccess;
    }

    #region GET
    public async Task<List<Usuario>> GetAllUsersAsync()
    {
        var list = await _agendaDA.GetAllUsersAsync();

        //Valida que la consulta no venga vacía
        if (list == null || list.Count == 0)
        {
            throw new InvalidOperationException("No existen usuarios registrados en la DB");
        }

        return list;
    }
    #endregion
}
