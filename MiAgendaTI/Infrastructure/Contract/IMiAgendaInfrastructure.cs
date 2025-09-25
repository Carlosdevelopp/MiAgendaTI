using DataAccess.Models.Tables;

namespace Infrastructure.Contract
{
    public interface IMiAgendaInfrastructure
    {
        Task<List<Usuarios_ET>> GetUsuariosList();
    }
}
