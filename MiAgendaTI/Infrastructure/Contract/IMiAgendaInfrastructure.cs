using DataAccess.Models.Tables;

namespace Infrastructure.Contract;

public interface IMiAgendaInfrastructure
{
    #region GET

    Task<Usuario?> LoginAsync(string credential, string password);

    Task<(bool Success, string Message)> RegisterAsync(Usuario model);

    Task<List<Contacto>> GetContactByIdAsync(int usuarioId);
    public int CalcularEdad(DateTime FechaNacimiento);
    #endregion
}
