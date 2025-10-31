using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Repositories
{
    public interface IUsuarioRepository
    {
        Task<Usuario> ObterPorIdAsync(Guid id);
        Task<Usuario> ObterPorEmailAsync(Email email);
        Task<IEnumerable<Usuario>> ObterTodosAsync();
        Task<IEnumerable<Usuario>> ObterAtivosAsync();
        Task<IEnumerable<Usuario>> ObterComEmprestimosAtivosAsync();
        Task<IEnumerable<Usuario>> BuscarPorNomeAsync(string nome);
        Task<bool> ExisteComEmailAsync(Email email);
        Task<bool> ExisteComEmailAsync(Email email, Guid excluirId);
        Task AdicionarAsync(Usuario usuario);
        Task AtualizarAsync(Usuario usuario);
        Task RemoverAsync(Usuario usuario);
        Task<int> ContarTotalAsync();
        Task<int> ContarAtivosAsync();
    }
}

