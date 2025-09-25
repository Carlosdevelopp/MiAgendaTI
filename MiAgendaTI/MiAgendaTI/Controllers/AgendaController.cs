using DataAccess.Models.Tables;
using Infrastructure.Contract;
using Microsoft.AspNetCore.Mvc;

namespace MiAgendaTI.Controllers
{
    public class AgendaController : Controller
    {
        private readonly IMiAgendaInfrastructure _agendaInfrastructure;

        public AgendaController(IMiAgendaInfrastructure agendaInfrastructure, ILogger<AgendaController> logger)
        {
            _agendaInfrastructure = agendaInfrastructure;
        }

        #region GET
        public async Task<IActionResult> Index()
        {
            try
            {
                var usuarios = await _agendaInfrastructure.GetUsuariosList();
                return View("Index", usuarios);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocurrió un error al obtener los usuarios: " + ex.Message;
                return View(new List<Usuarios_ET>()); // devuelves lista vacía
            }
        }
        #endregion
    }
}
