using System;
using System.Threading.Tasks;
using Application.DTOs;
using Domain.Entities;
using Domain.Repositories;
using Domain.ValueObjects;

namespace Application.UseCases.Livros
{
    public class CriarLivroUseCase
    {
        private readonly ILivroRepository _livroRepository;

        public CriarLivroUseCase(ILivroRepository livroRepository)
        {
            _livroRepository = livroRepository ?? throw new ArgumentNullException(nameof(livroRepository));
        }

        public async Task<LivroDto> ExecutarAsync(CriarLivroDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var isbn = ISBN.Create(dto.ISBN);

            // Verificar se já existe livro com este ISBN
            if (await _livroRepository.ExisteComISBNAsync(isbn))
                throw new InvalidOperationException("Já existe um livro cadastrado com este ISBN");

            var livro = new Livro(
                dto.Titulo,
                dto.Autor,
                isbn,
                dto.DataPublicacao,
                dto.Editora,
                dto.NumeroPaginas,
                dto.Genero,
                dto.Descricao
            );

            await _livroRepository.AdicionarAsync(livro);

            return new LivroDto
            {
                Id = livro.Id,
                Titulo = livro.Titulo,
                Autor = livro.Autor,
                ISBN = livro.ISBN,
                DataPublicacao = livro.DataPublicacao,
                Editora = livro.Editora,
                NumeroPaginas = livro.NumeroPaginas,
                Genero = livro.Genero,
                Disponivel = livro.Disponivel,
                Descricao = livro.Descricao,
                EstaEmprestado = livro.EstaEmprestado(),
                TotalEmprestimos = livro.TotalEmprestimos(),
                CreatedAt = livro.CreatedAt,
                UpdatedAt = livro.UpdatedAt
            };
        }
    }
}

