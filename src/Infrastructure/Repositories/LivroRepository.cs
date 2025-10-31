using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Domain.Entities;
using Domain.Repositories;
using Domain.ValueObjects;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class LivroRepository : ILivroRepository
    {
        private readonly IMongoCollection<Livro> _livros;

        public LivroRepository(MongoDbContext context)
        {
            _livros = context?.Livros ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Livro> ObterPorIdAsync(Guid id)
        {
            return await _livros.Find(l => l.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Livro> ObterPorISBNAsync(ISBN isbn)
        {
            var isbnValue = isbn?.Value;
            return await _livros.Find(l => l.ISBN.Value == isbnValue).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Livro>> ObterTodosAsync()
        {
            return await _livros.Find(_ => true)
                .SortBy(l => l.Titulo)
                .ToListAsync();
        }

        public async Task<IEnumerable<Livro>> ObterDisponiveisAsync()
        {
            var livros = await _livros.Find(l => l.Disponivel).ToListAsync();
            return livros.Where(l => l.PodeSerEmprestado()).OrderBy(l => l.Titulo);
        }

        public async Task<IEnumerable<Livro>> ObterEmprestadosAsync()
        {
            var livros = await _livros.Find(_ => true).ToListAsync();
            return livros.Where(l => l.EstaEmprestado()).OrderBy(l => l.Titulo);
        }

        public async Task<IEnumerable<Livro>> BuscarPorTituloAsync(string titulo)
        {
            var filter = Builders<Livro>.Filter.Regex(l => l.Titulo, new MongoDB.Bson.BsonRegularExpression(titulo, "i"));
            return await _livros.Find(filter)
                .SortBy(l => l.Titulo)
                .ToListAsync();
        }

        public async Task<IEnumerable<Livro>> BuscarPorAutorAsync(string autor)
        {
            var filter = Builders<Livro>.Filter.Regex(l => l.Autor, new MongoDB.Bson.BsonRegularExpression(autor, "i"));
            return await _livros.Find(filter)
                .SortBy(l => l.Titulo)
                .ToListAsync();
        }

        public async Task<IEnumerable<Livro>> BuscarPorGeneroAsync(string genero)
        {
            var filter = Builders<Livro>.Filter.Regex(l => l.Genero, new MongoDB.Bson.BsonRegularExpression(genero, "i"));
            return await _livros.Find(filter)
                .SortBy(l => l.Titulo)
                .ToListAsync();
        }

        public async Task<IEnumerable<Livro>> BuscarPorEditoraAsync(string editora)
        {
            var filter = Builders<Livro>.Filter.Regex(l => l.Editora, new MongoDB.Bson.BsonRegularExpression(editora, "i"));
            return await _livros.Find(filter)
                .SortBy(l => l.Titulo)
                .ToListAsync();
        }

        public async Task<IEnumerable<Livro>> ObterLancamentosRecentesAsync(int meses = 12)
        {
            var dataLimite = DateTime.Today.AddMonths(-meses);
            return await _livros.Find(l => l.DataPublicacao >= dataLimite)
                .SortByDescending(l => l.DataPublicacao)
                .ToListAsync();
        }

        public async Task<bool> ExisteComISBNAsync(ISBN isbn)
        {
            var isbnValue = isbn?.Value;
            var count = await _livros.CountDocumentsAsync(l => l.ISBN.Value == isbnValue);
            return count > 0;
        }

        public async Task<bool> ExisteComISBNAsync(ISBN isbn, Guid excluirId)
        {
            var isbnValue = isbn?.Value;
            var count = await _livros.CountDocumentsAsync(l => l.ISBN.Value == isbnValue && l.Id != excluirId);
            return count > 0;
        }

        public async Task AdicionarAsync(Livro livro)
        {
            await _livros.InsertOneAsync(livro);
        }

        public async Task AtualizarAsync(Livro livro)
        {
            await _livros.ReplaceOneAsync(l => l.Id == livro.Id, livro);
        }

        public async Task RemoverAsync(Livro livro)
        {
            await _livros.DeleteOneAsync(l => l.Id == livro.Id);
        }

        public async Task<int> ContarTotalAsync()
        {
            return (int)await _livros.CountDocumentsAsync(_ => true);
        }

        public async Task<int> ContarDisponiveisAsync()
        {
            var livros = await _livros.Find(l => l.Disponivel).ToListAsync();
            return livros.Count(l => l.PodeSerEmprestado());
        }

        public async Task<int> ContarEmprestadosAsync()
        {
            var livros = await _livros.Find(_ => true).ToListAsync();
            return livros.Count(l => l.EstaEmprestado());
        }
    }
}
