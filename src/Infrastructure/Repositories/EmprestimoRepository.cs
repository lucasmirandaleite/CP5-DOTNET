using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class EmprestimoRepository : IEmprestimoRepository
    {
        private readonly IMongoCollection<Emprestimo> _emprestimos;

        public EmprestimoRepository(MongoDbContext context)
        {
            _emprestimos = context?.Emprestimos ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Emprestimo> ObterPorIdAsync(Guid id)
        {
            return await _emprestimos.Find(e => e.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Emprestimo>> ObterTodosAsync()
        {
            return await _emprestimos.Find(_ => true)
                .SortByDescending(e => e.DataEmprestimo)
                .ToListAsync();
        }

        public async Task<IEnumerable<Emprestimo>> ObterAtivosAsync()
        {
            return await _emprestimos.Find(e => e.DataDevolucao == null)
                .SortBy(e => e.DataPrevistaDevolucao)
                .ToListAsync();
        }

        public async Task<IEnumerable<Emprestimo>> ObterAtrasadosAsync()
        {
            var hoje = DateTime.UtcNow.Date;
            return await _emprestimos.Find(e => e.DataDevolucao == null && e.DataPrevistaDevolucao < hoje)
                .SortBy(e => e.DataPrevistaDevolucao)
                .ToListAsync();
        }

        public async Task<IEnumerable<Emprestimo>> ObterPorUsuarioAsync(Guid usuarioId)
        {
            return await _emprestimos.Find(e => e.UsuarioId == usuarioId)
                .SortByDescending(e => e.DataEmprestimo)
                .ToListAsync();
        }

        public async Task<IEnumerable<Emprestimo>> ObterPorLivroAsync(Guid livroId)
        {
            return await _emprestimos.Find(e => e.LivroId == livroId)
                .SortByDescending(e => e.DataEmprestimo)
                .ToListAsync();
        }

        public async Task<IEnumerable<Emprestimo>> ObterAtivosDoUsuarioAsync(Guid usuarioId)
        {
            return await _emprestimos.Find(e => e.UsuarioId == usuarioId && e.DataDevolucao == null)
                .SortBy(e => e.DataPrevistaDevolucao)
                .ToListAsync();
        }

        public async Task<IEnumerable<Emprestimo>> ObterHistoricoDoUsuarioAsync(Guid usuarioId)
        {
            return await _emprestimos.Find(e => e.UsuarioId == usuarioId && e.DataDevolucao != null)
                .SortByDescending(e => e.DataDevolucao)
                .ToListAsync();
        }

        public async Task<IEnumerable<Emprestimo>> ObterHistoricoDoLivroAsync(Guid livroId)
        {
            return await _emprestimos.Find(e => e.LivroId == livroId && e.DataDevolucao != null)
                .SortByDescending(e => e.DataDevolucao)
                .ToListAsync();
        }

        public async Task<Emprestimo> ObterEmprestimoAtivoDoLivroAsync(Guid livroId)
        {
            return await _emprestimos.Find(e => e.LivroId == livroId && e.DataDevolucao == null)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Emprestimo>> ObterVencendoEmAsync(int dias)
        {
            var dataLimite = DateTime.UtcNow.Date.AddDays(dias);
            return await _emprestimos.Find(e => e.DataDevolucao == null && e.DataPrevistaDevolucao <= dataLimite)
                .SortBy(e => e.DataPrevistaDevolucao)
                .ToListAsync();
        }

        public async Task<IEnumerable<Emprestimo>> ObterPorPeriodoAsync(DateTime dataInicio, DateTime dataFim)
        {
            return await _emprestimos.Find(e => e.DataEmprestimo >= dataInicio && e.DataEmprestimo <= dataFim)
                .SortByDescending(e => e.DataEmprestimo)
                .ToListAsync();
        }

        public async Task AdicionarAsync(Emprestimo emprestimo)
        {
            await _emprestimos.InsertOneAsync(emprestimo);
        }

        public async Task AtualizarAsync(Emprestimo emprestimo)
        {
            await _emprestimos.ReplaceOneAsync(e => e.Id == emprestimo.Id, emprestimo);
        }

        public async Task RemoverAsync(Emprestimo emprestimo)
        {
            await _emprestimos.DeleteOneAsync(e => e.Id == emprestimo.Id);
        }

        public async Task<int> ContarTotalAsync()
        {
            return (int)await _emprestimos.CountDocumentsAsync(_ => true);
        }

        public async Task<int> ContarAtivosAsync()
        {
            return (int)await _emprestimos.CountDocumentsAsync(e => e.DataDevolucao == null);
        }

        public async Task<int> ContarAtrasadosAsync()
        {
            var hoje = DateTime.UtcNow.Date;
            return (int)await _emprestimos.CountDocumentsAsync(e => e.DataDevolucao == null && e.DataPrevistaDevolucao < hoje);
        }

        public async Task<int> ContarPorUsuarioAsync(Guid usuarioId)
        {
            return (int)await _emprestimos.CountDocumentsAsync(e => e.UsuarioId == usuarioId);
        }
    }
}
