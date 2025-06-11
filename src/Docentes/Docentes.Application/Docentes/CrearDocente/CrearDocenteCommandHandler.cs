using Docentes.Application.Abstractions.Messaging;
using Docentes.Application.Services;
using Docentes.Domain.Abstractions;
using Docentes.Domain.Docentes;

namespace Docentes.Application.Docentes.CrearDocente;

internal sealed class CrearDocenteCommandHandler :
ICommandHandler<CrearDocenteCommand, Guid>
{
    private readonly IDocenteRepository _docenteRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUsuarioService _usuarioService;
    private readonly ICursoService _cursoService;
    private readonly ICacheService _cacheService;

    public CrearDocenteCommandHandler(
        IDocenteRepository docenteRepository,
        IUnitOfWork unitOfWork,
        IUsuarioService usuarioService,
        ICursoService cursoService,
        ICacheService cacheService)
    {
        _docenteRepository = docenteRepository;
        _unitOfWork = unitOfWork;
        _usuarioService = usuarioService;
        _cursoService = cursoService;
        _cacheService = cacheService;
    }

    public async Task<Result<Guid>> Handle(CrearDocenteCommand request, CancellationToken cancellationToken)
    {
        if (!await _usuarioService.UsuarioExisteAsync(request.usuarioId, cancellationToken))
        {
            return Result.Failure<Guid>(new Error("UsuarioNotFound", "El usuario no existe."));
        }

        var cacheKey = $"curso_{request.especialidadId}";
        var cursoExiste = await _cacheService.GetCacheValueAsync<bool>(cacheKey, cancellationToken); // buscar cache

        if (!cursoExiste)
        {
            cursoExiste = await _cursoService.CursoExisteAsync(request.especialidadId, cancellationToken); // service real (BD)
            var expiration = TimeSpan.FromMinutes(5); // Expiración de 30 minutos
            await _cacheService.SetCacheValueAsync(cacheKey, cursoExiste, expiration, cancellationToken); // guardar cache
        }

        if (!cursoExiste)
        {
            return Result.Failure<Guid>(new Error("CursoNotFound", "El curso no existe."));
        }

        var docente = Docente.Create(
            request.usuarioId,
            request.especialidadId
        );

        _docenteRepository.Add(docente.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return docente.Value.Id;
    }
}