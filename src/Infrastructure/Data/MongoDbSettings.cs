namespace Infrastructure.Data
{
    public class MongoDbSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
        public string UsuariosCollectionName { get; set; } = "usuarios";
        public string LivrosCollectionName { get; set; } = "livros";
        public string EmprestimosCollectionName { get; set; } = "emprestimos";
    }
}
