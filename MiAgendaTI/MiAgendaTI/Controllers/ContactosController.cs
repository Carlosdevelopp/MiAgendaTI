using Infrastructure.Contract;
using Microsoft.AspNetCore.Mvc;

namespace MiAgendaTI.Controllers;

public class ContactosController : Controller
{
    private readonly IMiAgendaInfrastructure _miAgendaInfrastructure;

    public ContactosController(IMiAgendaInfrastructure miAgendaInfrastructure)
    {
        _miAgendaInfrastructure = miAgendaInfrastructure;
    }

    #region GET
    public IActionResult Index()
    {
        return View();
    }
    #endregion
}
