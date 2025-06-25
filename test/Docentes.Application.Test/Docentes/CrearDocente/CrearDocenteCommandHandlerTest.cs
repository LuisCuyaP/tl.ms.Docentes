using Docentes.Application.Docentes.CrearDocente;
using Docentes.Application.Services;
using Docentes.Domain.Abstractions;
using Docentes.Domain.Docentes;
using FluentAssertions;
using Moq;

namespace Docentes.Application.Test.Docentes.CrearDocente;

public class CrearDocenteCommandHandlerTest
{
    private readonly Mock<IDocenteRepository> _docenteRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IUsuarioService> _usuarioServiceMock;
    private readonly Mock<ICursoService> _cursoServiceMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly CrearDocenteCommandHandler _handler;

    public CrearDocenteCommandHandlerTest()
    {
        _docenteRepositoryMock = new Mock<IDocenteRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _usuarioServiceMock = new Mock<IUsuarioService>();
        _cursoServiceMock = new Mock<ICursoService>();
        _cacheServiceMock = new Mock<ICacheService>();

        _handler = new CrearDocenteCommandHandler(
            _docenteRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _usuarioServiceMock.Object,
            _cursoServiceMock.Object,
            _cacheServiceMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenUsuarioDoesNotExist()
    {
        var request = new CrearDocenteCommand(Guid.NewGuid(), Guid.NewGuid());
        _usuarioServiceMock
            .Setup(x => x.UsuarioExisteAsync(request.usuarioId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var result = await _handler.Handle(request, CancellationToken.None);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("UsuarioNotFound");
    }

    [Fact]
    public async Task Handle_ShouldUseCache_WhenCursoExist()
    {
        var request = new CrearDocenteCommand(Guid.NewGuid(), Guid.NewGuid());
        _usuarioServiceMock
            .Setup(x => x.UsuarioExisteAsync(request.usuarioId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _cacheServiceMock
            .Setup(x => x.GetCacheValueAsync<bool>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var result = await _handler.Handle(request, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        _cursoServiceMock.Verify(x => x.CursoExisteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}