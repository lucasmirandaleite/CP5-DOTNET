using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using Application.UseCases.Emprestimos;
using System;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class EmprestimosController : ControllerBase
    {
        private readonly CriarEmprestimoUseCase _criarEmprestimoUseCase;

        public EmprestimosController(CriarEmprestimoUseCase criarEmprestimoUseCase)
        {
            _criarEmprestimoUseCase = criarEmprestimoUseCase ?? throw new ArgumentNullException(nameof(criarEmprestimoUseCase));
        }

        /// <summary>
        /// Cria um novo empréstimo
        /// </summary>
        /// <param name="dto">Dados do empréstimo a ser criado</param>
        /// <returns>Empréstimo criado</returns>
        [HttpPost]
        [ProducesResponseType(typeof(EmprestimoDto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<EmprestimoDto>> Criar([FromBody] CriarEmprestimoDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var emprestimo = await _criarEmprestimoUseCase.ExecutarAsync(dto);
                return CreatedAtAction(nameof(ObterPorId), new { id = emprestimo.Id }, emprestimo);
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
        /// Obtém um empréstimo por ID (placeholder)
        /// </summary>
        /// <param name="id">ID do empréstimo</param>
        /// <returns>Dados do empréstimo</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EmprestimoDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<EmprestimoDto>> ObterPorId(Guid id)
        {
            // Implementação simplificada para demonstração
            return NotFound(new { message = "Endpoint não implementado nesta versão" });
        }
    }
}

