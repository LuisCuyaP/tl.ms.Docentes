using Docentes.Application.Services;

namespace Docentes.Infrastructure.Services;

public class CursoService : ICursoService
{
    private readonly HttpClient _httpClient;

    public CursoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> CursoExisteAsync(Guid cursoId, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"cursos/{cursoId}", cancellationToken);  
        return response.IsSuccessStatusCode;
    }
}