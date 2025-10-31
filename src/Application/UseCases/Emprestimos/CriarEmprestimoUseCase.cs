using System;
using System.Threading.Tasks;
using Application.DTOs;
using Domain.Entities;
using Domain.Repositories;

namespace Application.UseCases.Emprestimos
{
    public class CriarEmprestimoUseCase
    {
        private readonly IEmprestimoRepository _emprestimoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ILivroRepository _livroRepository;

        public CriarEmprestimoUseCase(
            IEmprestimoRepository emprestimoRepository,
            IUsuarioRepository usuarioRepository,
            ILivroRepository livroRepository)
        {
            _emprestimoRepository = emprestimoRepository ?? throw new ArgumentNullException(nameof(emprestimoRepository));
            _usuarioRepository = usuarioRepository ?? throw new ArgumentNullException(nameof(usuarioRepository));
            _livroRepository = livroRepository ?? throw new ArgumentNullException(nameof(livroRepository));
        }

        public async Task<EmprestimoDto> ExecutarAsync(CriarEmprestimoDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            // Buscar usuário
            var usuario = await _usuarioRepository.ObterPorIdAsync(dto.UsuarioId);
            if (usuario == null)
                throw new InvalidOperationException("Usuário não encontrado");

            // Buscar livro
            var livro = await _livroRepository.ObterPorIdAsync(dto.LivroId);
            if (livro == null)
                throw new InvalidOperationException("Livro não encontrado");

            // Verificar se o livro já está emprestado
            var emprestimoAtivo = await _emprestimoRepository.ObterEmprestimoAtivoDoLivroAsync(dto.LivroId);
            if (emprestimoAtivo != null)
                throw new InvalidOperationException("Livro já está emprestado");

            // Criar empréstimo (as validações de negócio estão nas entidades)
            var emprestimo = new Emprestimo(usuario, livro, dto.DiasEmprestimo);

            // Adicionar empréstimo aos agregados
            usuario.AdicionarEmprestimo(emprestimo);
            livro.AdicionarEmprestimo(emprestimo);

            // Persistir
            await _emprestimoRepository.AdicionarAsync(emprestimo);
            await _usuarioRepository.AtualizarAsync(usuario);
            await _livroRepository.AtualizarAsync(livro);

            return new EmprestimoDto
            {
                Id = emprestimo.Id,
                UsuarioId = emprestimo.UsuarioId,
                LivroId = emprestimo.LivroId,
                NomeUsuario = usuario.Nome,
                TituloLivro = livro.Titulo,
                DataEmprestimo = emprestimo.DataEmprestimo,
                DataPrevistaDevolucao = emprestimo.DataPrevistaDevolucao,
                DataDevolucao = emprestimo.DataDevolucao,
                Renovado = emprestimo.Renovado,
                DiasRenovacao = emprestimo.DiasRenovacao,
                Observacoes = emprestimo.Observacoes,
                EstaAtivo = emprestimo.EstaAtivo(),
                EstaAtrasado = emprestimo.EstaAtrasado(),
                DiasAtraso = emprestimo.DiasAtraso(),
                DiasRestantes = emprestimo.DiasRestantes(),
                MultaCalculada = emprestimo.CalcularMulta(),
                CreatedAt = emprestimo.CreatedAt,
                UpdatedAt = emprestimo.UpdatedAt
            };
        }
    }
}

