using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Common;
using Domain.ValueObjects;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities
{
    public class Livro : BaseEntity
    {
        private readonly List<Emprestimo> _emprestimos = new();

        [BsonElement("titulo")]
        public string Titulo { get; private set; } = string.Empty;
        [BsonElement("autor")]
        public string Autor { get; private set; } = string.Empty;
        [BsonElement("isbn")]
        public ISBN ISBN { get; private set; } = null!;
        [BsonElement("dataPublicacao")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime DataPublicacao { get; private set; }
        [BsonElement("editora")]
        public string Editora { get; private set; } = string.Empty;
        [BsonElement("numeroPaginas")]
        public int NumeroPaginas { get; private set; }
        [BsonElement("genero")]
        public string Genero { get; private set; } = string.Empty;
        [BsonElement("disponivel")]
        public bool Disponivel { get; private set; }
        [BsonElement("descricao")]
        public string Descricao { get; private set; } = string.Empty;

        [BsonElement("emprestimos")]
        [BsonIgnoreIfNull]
        public IReadOnlyCollection<Emprestimo> Emprestimos => _emprestimos.AsReadOnly();

        private Livro() { } // Para EF Core

        public Livro(string titulo, string autor, ISBN isbn, DateTime dataPublicacao, 
                     string editora, int numeroPaginas, string genero, string descricao = null)
        {
            ValidarTitulo(titulo);
            ValidarAutor(autor);
            ValidarDataPublicacao(dataPublicacao);
            ValidarEditora(editora);
            ValidarNumeroPaginas(numeroPaginas);
            ValidarGenero(genero);

            Titulo = titulo;
            Autor = autor;
            ISBN = isbn ?? throw new ArgumentNullException(nameof(isbn));
            DataPublicacao = dataPublicacao;
            Editora = editora;
            NumeroPaginas = numeroPaginas;
            Genero = genero;
            Descricao = descricao;
            Disponivel = true;
        }

        public void AtualizarTitulo(string novoTitulo)
        {
            ValidarTitulo(novoTitulo);
            Titulo = novoTitulo;
            SetUpdatedAt();
        }

        public void AtualizarAutor(string novoAutor)
        {
            ValidarAutor(novoAutor);
            Autor = novoAutor;
            SetUpdatedAt();
        }

        public void AtualizarDescricao(string novaDescricao)
        {
            Descricao = novaDescricao;
            SetUpdatedAt();
        }

        public void AtualizarGenero(string novoGenero)
        {
            ValidarGenero(novoGenero);
            Genero = novoGenero;
            SetUpdatedAt();
        }

        public bool PodeSerEmprestado()
        {
            return Disponivel && !EstaEmprestado();
        }

        public bool EstaEmprestado()
        {
            return _emprestimos.Any(e => e.EstaAtivo());
        }

        public Emprestimo EmprestimoAtivo()
        {
            return _emprestimos.FirstOrDefault(e => e.EstaAtivo());
        }

        public void MarcarComoIndisponivel()
        {
            if (EstaEmprestado())
                throw new InvalidOperationException("Não é possível marcar como indisponível um livro emprestado");

            Disponivel = false;
            SetUpdatedAt();
        }

        public void MarcarComoDisponivel()
        {
            Disponivel = true;
            SetUpdatedAt();
        }

        public void AdicionarEmprestimo(Emprestimo emprestimo)
        {
            if (emprestimo == null)
                throw new ArgumentNullException(nameof(emprestimo));

            if (!PodeSerEmprestado())
                throw new InvalidOperationException("Livro não pode ser emprestado no momento");

            _emprestimos.Add(emprestimo);
            SetUpdatedAt();
        }

        public int CalcularIdadeEmAnos()
        {
            return DateTime.Today.Year - DataPublicacao.Year;
        }

        public bool EhLancamentoRecente(int mesesParaConsiderarRecente = 12)
        {
            return DataPublicacao >= DateTime.Today.AddMonths(-mesesParaConsiderarRecente);
        }

        public int TotalEmprestimos()
        {
            return _emprestimos.Count;
        }

        public DateTime? DataUltimoEmprestimo()
        {
            return _emprestimos.OrderByDescending(e => e.DataEmprestimo).FirstOrDefault()?.DataEmprestimo;
        }

        private static void ValidarTitulo(string titulo)
        {
            if (string.IsNullOrWhiteSpace(titulo))
                throw new ArgumentException("Título não pode ser vazio", nameof(titulo));

            if (titulo.Length < 1)
                throw new ArgumentException("Título deve ter pelo menos 1 caractere", nameof(titulo));

            if (titulo.Length > 200)
                throw new ArgumentException("Título não pode ter mais de 200 caracteres", nameof(titulo));
        }

        private static void ValidarAutor(string autor)
        {
            if (string.IsNullOrWhiteSpace(autor))
                throw new ArgumentException("Autor não pode ser vazio", nameof(autor));

            if (autor.Length < 2)
                throw new ArgumentException("Autor deve ter pelo menos 2 caracteres", nameof(autor));

            if (autor.Length > 100)
                throw new ArgumentException("Autor não pode ter mais de 100 caracteres", nameof(autor));
        }

        private static void ValidarDataPublicacao(DateTime dataPublicacao)
        {
            if (dataPublicacao > DateTime.Today)
                throw new ArgumentException("Data de publicação não pode ser futura", nameof(dataPublicacao));

            if (dataPublicacao < new DateTime(1450, 1, 1)) // Invenção da imprensa
                throw new ArgumentException("Data de publicação muito antiga", nameof(dataPublicacao));
        }

        private static void ValidarEditora(string editora)
        {
            if (string.IsNullOrWhiteSpace(editora))
                throw new ArgumentException("Editora não pode ser vazia", nameof(editora));

            if (editora.Length > 100)
                throw new ArgumentException("Editora não pode ter mais de 100 caracteres", nameof(editora));
        }

        private static void ValidarNumeroPaginas(int numeroPaginas)
        {
            if (numeroPaginas < 1)
                throw new ArgumentException("Número de páginas deve ser maior que zero", nameof(numeroPaginas));

            if (numeroPaginas > 10000)
                throw new ArgumentException("Número de páginas muito alto", nameof(numeroPaginas));
        }

        private static void ValidarGenero(string genero)
        {
            if (string.IsNullOrWhiteSpace(genero))
                throw new ArgumentException("Gênero não pode ser vazio", nameof(genero));

            if (genero.Length > 50)
                throw new ArgumentException("Gênero não pode ter mais de 50 caracteres", nameof(genero));
        }
    }
}

