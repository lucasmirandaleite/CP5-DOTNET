using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using Application.UseCases.Usuarios;
using System;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class UsuariosController : ControllerBase
    {
        private readonly CriarUsuarioUseCase _criarUsuarioUseCase;
        private readonly ObterUsuarioUseCase _obterUsuarioUseCase;

        public UsuariosController(
            CriarUsuarioUseCase criarUsuarioUseCase,
            ObterUsuarioUseCase obterUsuarioUseCase)
        {
            _criarUsuarioUseCase = criarUsuarioUseCase ?? throw new ArgumentNullException(nameof(criarUsuarioUseCase));
            _obterUsuarioUseCase = obterUsuarioUseCase ?? throw new ArgumentNullException(nameof(obterUsuarioUseCase));
        }

        /// <summary>
        /// Obtém todos os usuários
        /// </summary>
        /// <returns>Lista de usuários</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UsuarioResumoDto>), 200)]
        public async Task<ActionResult<IEnumerable<UsuarioResumoDto>>> ObterTodos()
        {
            try
            {
                var usuarios = await _obterUsuarioUseCase.ObterTodosAsync();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", details = ex.Message });
            }
        }

        /// <summary>
        /// Obtém um usuário por ID
        /// </summary>
        /// <param name="id">ID do usuário</param>
        /// <returns>Dados do usuário</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UsuarioDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<UsuarioDto>> ObterPorId(Guid id)
        {
            try
            {
                var usuario = await _obterUsuarioUseCase.ObterPorIdAsync(id);
                return Ok(usuario);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", details = ex.Message });
            }
        }

        /// <summary>
        /// Busca usuários por nome
        /// </summary>
        /// <param name="nome">Nome ou parte do nome para buscar</param>
        /// <returns>Lista de usuários encontrados</returns>
        [HttpGet("buscar")]
        [ProducesResponseType(typeof(IEnumerable<UsuarioResumoDto>), 200)]
        public async Task<ActionResult<IEnumerable<UsuarioResumoDto>>> BuscarPorNome([FromQuery] string nome)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nome))
                    return BadRequest(new { message = "Nome para busca é obrigatório" });

                var usuarios = await _obterUsuarioUseCase.BuscarPorNomeAsync(nome);
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", details = ex.Message });
            }
        }

        /// <summary>
        /// Cria um novo usuário
        /// </summary>
        /// <param name="dto">Dados do usuário a ser criado</param>
        /// <returns>Usuário criado</returns>
        [HttpPost]
        [ProducesResponseType(typeof(UsuarioDto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<UsuarioDto>> Criar([FromBody] CriarUsuarioDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var usuario = await _criarUsuarioUseCase.ExecutarAsync(dto);
                return CreatedAtAction(nameof(ObterPorId), new { id = usuario.Id }, usuario);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", details = ex.Message });
            }
        }
    }
}

