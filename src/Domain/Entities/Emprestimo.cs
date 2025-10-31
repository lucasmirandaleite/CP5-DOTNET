using System;
using Domain.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities
{
    public class Emprestimo : BaseEntity
    {
        [BsonElement("usuarioId")]
        [BsonRepresentation(BsonType.String)]
        public Guid UsuarioId { get; private set; }
        [BsonElement("livroId")]
        [BsonRepresentation(BsonType.String)]
        public Guid LivroId { get; private set; }
        [BsonElement("dataEmprestimo")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime DataEmprestimo { get; private set; }
        [BsonElement("dataPrevistaDevolucao")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime DataPrevistaDevolucao { get; private set; }
        [BsonElement("dataDevolucao")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? DataDevolucao { get; private set; }
        [BsonElement("renovado")]
        public bool Renovado { get; private set; }
        [BsonElement("diasRenovacao")]
        public int DiasRenovacao { get; private set; }
        [BsonElement("observacoes")]
        public string Observacoes { get; private set; } = string.Empty;

        // Navigation properties
        [BsonIgnore]
        public Usuario Usuario { get; private set; } = null!; // Inicializado no construtor público
        [BsonIgnore]
        public Livro Livro { get; private set; } = null!; // Inicializado no construtor público

        private Emprestimo() { } // Para EF Core

        public Emprestimo(Guid usuarioId, Guid livroId, int diasEmprestimo = 14)
        {
            ValidarDiasEmprestimo(diasEmprestimo);

            UsuarioId = usuarioId;
            LivroId = livroId;
            DataEmprestimo = DateTime.UtcNow;
            DataPrevistaDevolucao = DataEmprestimo.AddDays(diasEmprestimo);
            Renovado = false;
            DiasRenovacao = 0;
        }

        public Emprestimo(Usuario usuario, Livro livro, int diasEmprestimo = 14)
        {
            ValidarUsuario(usuario);
            ValidarLivro(livro);
            ValidarDiasEmprestimo(diasEmprestimo);

            UsuarioId = usuario.Id;
            LivroId = livro.Id;
            Usuario = usuario;
            Livro = livro;
            DataEmprestimo = DateTime.UtcNow;
            DataPrevistaDevolucao = DataEmprestimo.AddDays(diasEmprestimo);
            Renovado = false;
            DiasRenovacao = 0;
        }

        public bool EstaAtivo()
        {
            return DataDevolucao == null;
        }

        public bool EstaAtrasado()
        {
            return EstaAtivo() && DateTime.UtcNow.Date > DataPrevistaDevolucao.Date;
        }

        public int DiasAtraso()
        {
            if (!EstaAtrasado())
                return 0;

            return (DateTime.UtcNow.Date - DataPrevistaDevolucao.Date).Days;
        }

        public int DiasRestantes()
        {
            if (!EstaAtivo())
                return 0;

            var dias = (DataPrevistaDevolucao.Date - DateTime.UtcNow.Date).Days;
            return Math.Max(0, dias);
        }

        public void Renovar(int diasAdicionais = 14)
        {
            if (!EstaAtivo())
                throw new InvalidOperationException("Não é possível renovar um empréstimo já devolvido");

            if (Renovado)
                throw new InvalidOperationException("Empréstimo já foi renovado");

            if (EstaAtrasado())
                throw new InvalidOperationException("Não é possível renovar um empréstimo em atraso");

            ValidarDiasEmprestimo(diasAdicionais);

            DataPrevistaDevolucao = DataPrevistaDevolucao.AddDays(diasAdicionais);
            Renovado = true;
            DiasRenovacao = diasAdicionais;
            SetUpdatedAt();
        }

        public void Devolver(string observacoes = null)
        {
            if (!EstaAtivo())
                throw new InvalidOperationException("Empréstimo já foi devolvido");

            DataDevolucao = DateTime.UtcNow;
            Observacoes = observacoes;
            SetUpdatedAt();
        }

        public void AdicionarObservacoes(string observacoes)
        {
            if (string.IsNullOrWhiteSpace(observacoes))
                throw new ArgumentException("Observações não podem ser vazias", nameof(observacoes));

            if (observacoes.Length > 500)
                throw new ArgumentException("Observações não podem ter mais de 500 caracteres", nameof(observacoes));

            Observacoes = observacoes;
            SetUpdatedAt();
        }

        public TimeSpan DuracaoEmprestimo()
        {
            var dataFim = DataDevolucao ?? DateTime.UtcNow;
            return dataFim - DataEmprestimo;
        }

        public decimal CalcularMulta(decimal valorMultaDiaria = 2.00m)
        {
            if (!EstaAtrasado())
                return 0;

            return DiasAtraso() * valorMultaDiaria;
        }

        public bool PodeSerRenovado()
        {
            return EstaAtivo() && !Renovado && !EstaAtrasado();
        }

        private static void ValidarUsuario(Usuario usuario)
        {
            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario));

            if (!usuario.Ativo)
                throw new InvalidOperationException("Usuário deve estar ativo para realizar empréstimo");

            if (!usuario.PodeEmprestarLivro())
                throw new InvalidOperationException("Usuário atingiu o limite de empréstimos");
        }

        private static void ValidarLivro(Livro livro)
        {
            if (livro == null)
                throw new ArgumentNullException(nameof(livro));

            if (!livro.PodeSerEmprestado())
                throw new InvalidOperationException("Livro não está disponível para empréstimo");
        }

        private static void ValidarDiasEmprestimo(int dias)
        {
            if (dias < 1)
                throw new ArgumentException("Dias de empréstimo deve ser maior que zero", nameof(dias));

            if (dias > 90)
                throw new ArgumentException("Dias de empréstimo não pode ser maior que 90", nameof(dias));
        }
    }
}

