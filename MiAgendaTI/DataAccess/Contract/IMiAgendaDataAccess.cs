using DataAccess.Models.Tables;

namespace DataAccess.Contract
{
    public interface IMiAgendaDataAccess 
    {
        Task<List<Usuarios_ET>> GetUsuariosList();
    }
}
