using System;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class EmprestimoDto
    {
        public Guid Id { get; set; }
        public Guid UsuarioId { get; set; }
        public Guid LivroId { get; set; }
        public string NomeUsuario { get; set; }
        public string TituloLivro { get; set; }
        public DateTime DataEmprestimo { get; set; }
        public DateTime DataPrevistaDevolucao { get; set; }
        public DateTime? DataDevolucao { get; set; }
        public bool Renovado { get; set; }
        public int DiasRenovacao { get; set; }
        public string Observacoes { get; set; }
        public bool EstaAtivo { get; set; }
        public bool EstaAtrasado { get; set; }
        public int DiasAtraso { get; set; }
        public int DiasRestantes { get; set; }
        public decimal MultaCalculada { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class CriarEmprestimoDto
    {
        [Required(ErrorMessage = "ID do usuário é obrigatório")]
        public Guid UsuarioId { get; set; }

        [Required(ErrorMessage = "ID do livro é obrigatório")]
        public Guid LivroId { get; set; }

        [Range(1, 90, ErrorMessage = "Dias de empréstimo deve estar entre 1 e 90")]
        public int DiasEmprestimo { get; set; } = 14;
    }

    public class RenovarEmprestimoDto
    {
        [Range(1, 90, ErrorMessage = "Dias adicionais deve estar entre 1 e 90")]
        public int DiasAdicionais { get; set; } = 14;
    }

    public class DevolverEmprestimoDto
    {
        [StringLength(500, ErrorMessage = "Observações não podem ter mais de 500 caracteres")]
        public string Observacoes { get; set; }
    }

    public class EmprestimoResumoDto
    {
        public Guid Id { get; set; }
        public string NomeUsuario { get; set; }
        public string TituloLivro { get; set; }
        public DateTime DataEmprestimo { get; set; }
        public DateTime DataPrevistaDevolucao { get; set; }
        public bool EstaAtivo { get; set; }
        public bool EstaAtrasado { get; set; }
        public int DiasAtraso { get; set; }
    }
}

