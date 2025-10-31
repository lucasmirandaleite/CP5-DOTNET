using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using Application.UseCases.Livros;
using System;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class LivrosController : ControllerBase
    {
        private readonly CriarLivroUseCase _criarLivroUseCase;

        public LivrosController(CriarLivroUseCase criarLivroUseCase)
        {
            _criarLivroUseCase = criarLivroUseCase ?? throw new ArgumentNullException(nameof(criarLivroUseCase));
        }

        /// <summary>
        /// Cria um novo livro
        /// </summary>
        /// <param name="dto">Dados do livro a ser criado</param>
        /// <returns>Livro criado</returns>
        [HttpPost]
        [ProducesResponseType(typeof(LivroDto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<LivroDto>> Criar([FromBody] CriarLivroDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var livro = await _criarLivroUseCase.ExecutarAsync(dto);
                return CreatedAtAction(nameof(ObterPorId), new { id = livro.Id }, livro);
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

        /// <summary>
        /// Obtém um livro por ID (placeholder)
        /// </summary>
        /// <param name="id">ID do livro</param>
        /// <returns>Dados do livro</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(LivroDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<LivroDto>> ObterPorId(Guid id)
        {
            // Implementação simplificada para demonstração
            return NotFound(new { message = "Endpoint não implementado nesta versão" });
        }
    }
}

