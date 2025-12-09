using DataAccess.Models.Tables;
using Infrastructure.Contract;
using Microsoft.AspNetCore.Mvc;

namespace MiAgendaTI.Controllers;

public class AgendaController : Controller
{
    private readonly IMiAgendaInfrastructure _agendaInfrastructure;

    public AgendaController(IMiAgendaInfrastructure agendaInfrastructure)
    {
        _agendaInfrastructure = agendaInfrastructure;
    }

    #region GET

    #endregion
}
