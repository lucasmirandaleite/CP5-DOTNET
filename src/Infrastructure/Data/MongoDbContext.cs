using Domain.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Infrastructure.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;
        private readonly MongoDbSettings _settings;

        public MongoDbContext(IOptions<MongoDbSettings> settings)
        {
            _settings = settings.Value;
            var client = new MongoClient(_settings.ConnectionString);
            _database = client.GetDatabase(_settings.DatabaseName);
        }

        public IMongoCollection<Usuario> Usuarios => 
            _database.GetCollection<Usuario>(_settings.UsuariosCollectionName);

        public IMongoCollection<Livro> Livros => 
            _database.GetCollection<Livro>(_settings.LivrosCollectionName);

        public IMongoCollection<Emprestimo> Emprestimos => 
            _database.GetCollection<Emprestimo>(_settings.EmprestimosCollectionName);

        public IMongoDatabase Database => _database;
    }
}
