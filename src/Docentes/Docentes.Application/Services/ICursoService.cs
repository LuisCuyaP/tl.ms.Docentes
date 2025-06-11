namespace Docentes.Application.Services;

public interface ICursoService
{
    Task<bool> CursoExisteAsync(Guid cursoId, CancellationToken cancellationToken);
}