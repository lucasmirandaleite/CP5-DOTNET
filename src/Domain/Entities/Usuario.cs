using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Common;
using Domain.ValueObjects;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities
{
    public class Usuario : BaseEntity
    {
        private readonly List<Emprestimo> _emprestimos = new();

        [BsonElement("nome")]
        public string Nome { get; private set; } = string.Empty;
        [BsonElement("email")]
        public Email Email { get; private set; } = null!;
        [BsonElement("dataNascimento")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime DataNascimento { get; private set; }
        [BsonElement("ativo")]
        public bool Ativo { get; private set; }
        [BsonElement("limiteEmprestimos")]
        public int LimiteEmprestimos { get; private set; }

        [BsonElement("emprestimos")]
        [BsonIgnoreIfNull]
        public IReadOnlyCollection<Emprestimo> Emprestimos => _emprestimos.AsReadOnly();

        private Usuario() { } // Para EF Core

        public Usuario(string nome, Email email, DateTime dataNascimento, int limiteEmprestimos = 3)
        {
            ValidarNome(nome);
            ValidarDataNascimento(dataNascimento);
            ValidarLimiteEmprestimos(limiteEmprestimos);

            Nome = nome;
            Email = email ?? throw new ArgumentNullException(nameof(email));
            DataNascimento = dataNascimento;
            LimiteEmprestimos = limiteEmprestimos;
            Ativo = true;
        }

        public void AtualizarNome(string novoNome)
        {
            ValidarNome(novoNome);
            Nome = novoNome;
            SetUpdatedAt();
        }

        public void AtualizarEmail(Email novoEmail)
        {
            Email = novoEmail ?? throw new ArgumentNullException(nameof(novoEmail));
            SetUpdatedAt();
        }

        public void Ativar()
        {
            Ativo = true;
            SetUpdatedAt();
        }

        public void Desativar()
        {
            if (PossuiEmprestimosAtivos())
                throw new InvalidOperationException("Não é possível desativar usuário com empréstimos ativos");

            Ativo = false;
            SetUpdatedAt();
        }

        public void AlterarLimiteEmprestimos(int novoLimite)
        {
            ValidarLimiteEmprestimos(novoLimite);
            
            if (novoLimite < EmprestimosAtivos().Count())
                throw new InvalidOperationException("Novo limite não pode ser menor que a quantidade de empréstimos ativos");

            LimiteEmprestimos = novoLimite;
            SetUpdatedAt();
        }

        public bool PodeEmprestarLivro()
        {
            return Ativo && EmprestimosAtivos().Count() < LimiteEmprestimos;
        }

        public bool PossuiEmprestimosAtivos()
        {
            return EmprestimosAtivos().Any();
        }

        public IEnumerable<Emprestimo> EmprestimosAtivos()
        {
            return _emprestimos.Where(e => e.EstaAtivo());
        }

        public void AdicionarEmprestimo(Emprestimo emprestimo)
        {
            if (emprestimo == null)
                throw new ArgumentNullException(nameof(emprestimo));

            if (!PodeEmprestarLivro())
                throw new InvalidOperationException("Usuário não pode realizar mais empréstimos");

            _emprestimos.Add(emprestimo);
            SetUpdatedAt();
        }

        public int CalcularIdade()
        {
            var hoje = DateTime.Today;
            var idade = hoje.Year - DataNascimento.Year;
            
            if (DataNascimento.Date > hoje.AddYears(-idade))
                idade--;

            return idade;
        }

        private static void ValidarNome(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome não pode ser vazio", nameof(nome));

            if (nome.Length < 2)
                throw new ArgumentException("Nome deve ter pelo menos 2 caracteres", nameof(nome));

            if (nome.Length > 100)
                throw new ArgumentException("Nome não pode ter mais de 100 caracteres", nameof(nome));
        }

        private static void ValidarDataNascimento(DateTime dataNascimento)
        {
            if (dataNascimento > DateTime.Today)
                throw new ArgumentException("Data de nascimento não pode ser futura", nameof(dataNascimento));

            var idade = DateTime.Today.Year - dataNascimento.Year;
            if (dataNascimento.Date > DateTime.Today.AddYears(-idade))
                idade--;

            if (idade < 0 || idade > 120)
                throw new ArgumentException("Idade deve estar entre 0 e 120 anos", nameof(dataNascimento));
        }

        private static void ValidarLimiteEmprestimos(int limite)
        {
            if (limite < 1 || limite > 10)
                throw new ArgumentException("Limite de empréstimos deve estar entre 1 e 10", nameof(limite));
        }
    }
}

