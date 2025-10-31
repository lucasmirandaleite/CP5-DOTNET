using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IEmprestimoRepository
    {
        Task<Emprestimo> ObterPorIdAsync(Guid id);
        Task<IEnumerable<Emprestimo>> ObterTodosAsync();
        Task<IEnumerable<Emprestimo>> ObterAtivosAsync();
        Task<IEnumerable<Emprestimo>> ObterAtrasadosAsync();
        Task<IEnumerable<Emprestimo>> ObterPorUsuarioAsync(Guid usuarioId);
        Task<IEnumerable<Emprestimo>> ObterPorLivroAsync(Guid livroId);
        Task<IEnumerable<Emprestimo>> ObterAtivosDoUsuarioAsync(Guid usuarioId);
        Task<IEnumerable<Emprestimo>> ObterHistoricoDoUsuarioAsync(Guid usuarioId);
        Task<IEnumerable<Emprestimo>> ObterHistoricoDoLivroAsync(Guid livroId);
        Task<Emprestimo> ObterEmprestimoAtivoDoLivroAsync(Guid livroId);
        Task<IEnumerable<Emprestimo>> ObterVencendoEmAsync(int dias);
        Task<IEnumerable<Emprestimo>> ObterPorPeriodoAsync(DateTime dataInicio, DateTime dataFim);
        Task AdicionarAsync(Emprestimo emprestimo);
        Task AtualizarAsync(Emprestimo emprestimo);
        Task RemoverAsync(Emprestimo emprestimo);
        Task<int> ContarTotalAsync();
        Task<int> ContarAtivosAsync();
        Task<int> ContarAtrasadosAsync();
        Task<int> ContarPorUsuarioAsync(Guid usuarioId);
    }
}

