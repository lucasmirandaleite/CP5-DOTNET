using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using Domain.Entities;
using Domain.Repositories;
using Domain.ValueObjects;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly IMongoCollection<Usuario> _usuarios;

        public UsuarioRepository(MongoDbContext context)
        {
            _usuarios = context?.Usuarios ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Usuario> ObterPorIdAsync(Guid id)
        {
            return await _usuarios.Find(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Usuario> ObterPorEmailAsync(Email email)
        {
            var emailValue = email?.Value;
            return await _usuarios.Find(u => u.Email.Value == emailValue).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Usuario>> ObterTodosAsync()
        {
            return await _usuarios.Find(_ => true)
                .SortBy(u => u.Nome)
                .ToListAsync();
        }

        public async Task<IEnumerable<Usuario>> ObterAtivosAsync()
        {
            return await _usuarios.Find(u => u.Ativo)
                .SortBy(u => u.Nome)
                .ToListAsync();
        }

        public async Task<IEnumerable<Usuario>> ObterComEmprestimosAtivosAsync()
        {
            var usuarios = await _usuarios.Find(_ => true).ToListAsync();
            return usuarios.Where(u => u.PossuiEmprestimosAtivos()).OrderBy(u => u.Nome);
        }

        public async Task<IEnumerable<Usuario>> BuscarPorNomeAsync(string nome)
        {
            var filter = Builders<Usuario>.Filter.Regex(u => u.Nome, new MongoDB.Bson.BsonRegularExpression(nome, "i"));
            return await _usuarios.Find(filter)
                .SortBy(u => u.Nome)
                .ToListAsync();
        }

        public async Task<bool> ExisteComEmailAsync(Email email)
        {
            var emailValue = email?.Value;
            var count = await _usuarios.CountDocumentsAsync(u => u.Email.Value == emailValue);
            return count > 0;
        }

        public async Task<bool> ExisteComEmailAsync(Email email, Guid excluirId)
        {
            var emailValue = email?.Value;
            var count = await _usuarios.CountDocumentsAsync(u => u.Email.Value == emailValue && u.Id != excluirId);
            return count > 0;
        }

        public async Task AdicionarAsync(Usuario usuario)
        {
            await _usuarios.InsertOneAsync(usuario);
        }

        public async Task AtualizarAsync(Usuario usuario)
        {
            await _usuarios.ReplaceOneAsync(u => u.Id == usuario.Id, usuario);
        }

        public async Task RemoverAsync(Usuario usuario)
        {
            await _usuarios.DeleteOneAsync(u => u.Id == usuario.Id);
        }

        public async Task<int> ContarTotalAsync()
        {
            return (int)await _usuarios.CountDocumentsAsync(_ => true);
        }

        public async Task<int> ContarAtivosAsync()
        {
            return (int)await _usuarios.CountDocumentsAsync(u => u.Ativo);
        }
    }
}
