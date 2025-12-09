using DataAccess.Contract;
using DataAccess.Models.Tables;
using Infrastructure.Contract;
using Isopoh.Cryptography.Argon2;

namespace Infrastructure.Implementation;

public class MiAgendaInfrastructure : IMiAgendaInfrastructure
{
    private readonly IMiAgendaDataAccess _miAgendaDataAccess;

    public MiAgendaInfrastructure(IMiAgendaDataAccess agendaDataAccess)
    {
        _miAgendaDataAccess = agendaDataAccess;
    }

    #region GET
    public async Task<Usuario?> LoginAsync(string credential, string password)
    {
        var usuario = await _miAgendaDataAccess.GetUserByCredentialAsync(credential);
        bool credentialCoincide = (usuario != null);
        if (usuario == null)
            return null;

        if (usuario.Password == password)
        {
            return usuario;
        }
        else
        {
            return null;
        }
    }

    public async Task<(bool Success, string Message)> RegisterAsync(Usuario model)
    {
        bool existe = await _miAgendaDataAccess.ExistsAsync(model.Correo, model.NombreUsuario);

        if (existe)
            return (false, "Ël correo o nombre de usuario ya está registrado.");

        var passwordHash = Argon2.Hash(model.Password);

        var Nuevousuario = new Usuario
        {
            Nombre = model.Nombre,
            PrimerApellido = model.PrimerApellido,
            SegundoApellido = model.SegundoApellido,
            Telefono = model.Telefono,
            Correo = model.Correo,
            NombreUsuario = model.NombreUsuario,
            Password = passwordHash
        };

        await _miAgendaDataAccess.RegisterUserAsync(Nuevousuario);

        return (true, "Usuario registrado correctamente.");
    }

    public async Task<List<Contacto>> GetContactByIdAsync(int usuarioId)
    {
        return await _miAgendaDataAccess.GetContactoByIdAsync(usuarioId);
    }

    public int CalcularEdad(DateTime FechaNacimiento)
    {
        var hoy = DateTime.Now;
        int edad = hoy.Year - FechaNacimiento.Year;
        if (FechaNacimiento.Date > hoy.AddYears(-edad)) edad--;
        return edad;
    }
    #endregion
}
