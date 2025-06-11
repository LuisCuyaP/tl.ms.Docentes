namespace Docentes.Application.Services;

public interface IUsuarioService
{
    Task<bool> UsuarioExisteAsync(Guid usuarioId, CancellationToken cancellationToken);
}