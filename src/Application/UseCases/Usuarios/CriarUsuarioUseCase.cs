using System;
using System.Threading.Tasks;
using Application.DTOs;
using Domain.Entities;
using Domain.Repositories;
using Domain.ValueObjects;

namespace Application.UseCases.Usuarios
{
    public class CriarUsuarioUseCase
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public CriarUsuarioUseCase(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository ?? throw new ArgumentNullException(nameof(usuarioRepository));
        }

        public async Task<UsuarioDto> ExecutarAsync(CriarUsuarioDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var email = Email.Create(dto.Email);

            // Verificar se já existe usuário com este email
            if (await _usuarioRepository.ExisteComEmailAsync(email))
                throw new InvalidOperationException("Já existe um usuário cadastrado com este email");

            var usuario = new Usuario(
                dto.Nome,
                email,
                dto.DataNascimento,
                dto.LimiteEmprestimos
            );

            await _usuarioRepository.AdicionarAsync(usuario);

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
    }
}

