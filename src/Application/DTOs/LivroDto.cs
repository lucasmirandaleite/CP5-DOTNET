using System;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class LivroDto
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string ISBN { get; set; }
        public DateTime DataPublicacao { get; set; }
        public string Editora { get; set; }
        public int NumeroPaginas { get; set; }
        public string Genero { get; set; }
        public bool Disponivel { get; set; }
        public string Descricao { get; set; }
        public bool EstaEmprestado { get; set; }
        public int TotalEmprestimos { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class CriarLivroDto
    {
        [Required(ErrorMessage = "Título é obrigatório")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Título deve ter entre 1 e 200 caracteres")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "Autor é obrigatório")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Autor deve ter entre 2 e 100 caracteres")]
        public string Autor { get; set; }

        [Required(ErrorMessage = "ISBN é obrigatório")]
        public string ISBN { get; set; }

        [Required(ErrorMessage = "Data de publicação é obrigatória")]
        public DateTime DataPublicacao { get; set; }

        [Required(ErrorMessage = "Editora é obrigatória")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Editora deve ter entre 1 e 100 caracteres")]
        public string Editora { get; set; }

        [Range(1, 10000, ErrorMessage = "Número de páginas deve estar entre 1 e 10000")]
        public int NumeroPaginas { get; set; }

        [Required(ErrorMessage = "Gênero é obrigatório")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Gênero deve ter entre 1 e 50 caracteres")]
        public string Genero { get; set; }

        [StringLength(1000, ErrorMessage = "Descrição não pode ter mais de 1000 caracteres")]
        public string Descricao { get; set; }
    }

    public class AtualizarLivroDto
    {
        [Required(ErrorMessage = "Título é obrigatório")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Título deve ter entre 1 e 200 caracteres")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "Autor é obrigatório")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Autor deve ter entre 2 e 100 caracteres")]
        public string Autor { get; set; }

        [Required(ErrorMessage = "Gênero é obrigatório")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Gênero deve ter entre 1 e 50 caracteres")]
        public string Genero { get; set; }

        [StringLength(1000, ErrorMessage = "Descrição não pode ter mais de 1000 caracteres")]
        public string Descricao { get; set; }
    }

    public class LivroResumoDto
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string ISBN { get; set; }
        public string Genero { get; set; }
        public bool Disponivel { get; set; }
        public bool EstaEmprestado { get; set; }
    }
}

