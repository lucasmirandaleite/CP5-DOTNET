using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Repositories
{
    public interface ILivroRepository
    {
        Task<Livro> ObterPorIdAsync(Guid id);
        Task<Livro> ObterPorISBNAsync(ISBN isbn);
        Task<IEnumerable<Livro>> ObterTodosAsync();
        Task<IEnumerable<Livro>> ObterDisponiveisAsync();
        Task<IEnumerable<Livro>> ObterEmprestadosAsync();
        Task<IEnumerable<Livro>> BuscarPorTituloAsync(string titulo);
        Task<IEnumerable<Livro>> BuscarPorAutorAsync(string autor);
        Task<IEnumerable<Livro>> BuscarPorGeneroAsync(string genero);
        Task<IEnumerable<Livro>> BuscarPorEditoraAsync(string editora);
        Task<IEnumerable<Livro>> ObterLancamentosRecentesAsync(int meses = 12);
        Task<bool> ExisteComISBNAsync(ISBN isbn);
        Task<bool> ExisteComISBNAsync(ISBN isbn, Guid excluirId);
        Task AdicionarAsync(Livro livro);
        Task AtualizarAsync(Livro livro);
        Task RemoverAsync(Livro livro);
        Task<int> ContarTotalAsync();
        Task<int> ContarDisponiveisAsync();
        Task<int> ContarEmprestadosAsync();
    }
}

