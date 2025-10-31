using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs;
using Domain.Repositories;

namespace Application.UseCases.Usuarios
{
    public class ObterUsuarioUseCase
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public ObterUsuarioUseCase(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository ?? throw new ArgumentNullException(nameof(usuarioRepository));
        }

        public async Task<UsuarioDto> ObterPorIdAsync(Guid id)
        {
            var usuario = await _usuarioRepository.ObterPorIdAsync(id);
            
            if (usuario == null)
                throw new InvalidOperationException("Usuário não encontrado");

            return new UsuarioDto
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                DataNascimento = usuario.DataNascimento,
                Ativo = usuario.Ativo,
                LimiteEmprestimos = usuario.LimiteEmprestimos,
                EmprestimosAtivos = usuario.EmprestimosAtivos().Count(),
                CreatedAt = usuario.CreatedAt,
                UpdatedAt = usuario.UpdatedAt
            };
        }

        public async Task<IEnumerable<UsuarioResumoDto>> ObterTodosAsync()
        {
            var usuarios = await _usuarioRepository.ObterTodosAsync();

            return usuarios.Select(u => new UsuarioResumoDto
            {
                Id = u.Id,
                Nome = u.Nome,
                Email = u.Email,
                Ativo = u.Ativo,
                EmprestimosAtivos = u.EmprestimosAtivos().Count()
            });
        }

        public async Task<IEnumerable<UsuarioResumoDto>> BuscarPorNomeAsync(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome para busca não pode ser vazio", nameof(nome));

            var usuarios = await _usuarioRepository.BuscarPorNomeAsync(nome);

            return usuarios.Select(u => new UsuarioResumoDto
            {
                Id = u.Id,
                Nome = u.Nome,
                Email = u.Email,
                Ativo = u.Ativo,
                EmprestimosAtivos = u.EmprestimosAtivos().Count()
            });
        }
    }
}

