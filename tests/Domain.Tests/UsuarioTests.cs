using System;
using Xunit;
using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Tests
{
    public class UsuarioTests
    {
        [Fact]
        public void CriarUsuario_ComDadosValidos_DeveCriarComSucesso()
        {
            // Arrange
            var nome = "João Silva";
            var email = Email.Create("joao@email.com");
            var dataNascimento = new DateTime(1990, 5, 15);
            var limiteEmprestimos = 3;

            // Act
            var usuario = new Usuario(nome, email, dataNascimento, limiteEmprestimos);

            // Assert
            Assert.NotNull(usuario);
            Assert.Equal(nome, usuario.Nome);
            Assert.Equal(email, usuario.Email);
            Assert.Equal(dataNascimento, usuario.DataNascimento);
            Assert.Equal(limiteEmprestimos, usuario.LimiteEmprestimos);
            Assert.True(usuario.Ativo);
        }

        [Fact]
        public void CriarUsuario_ComNomeVazio_DeveLancarExcecao()
        {
            // Arrange
            var email = Email.Create("joao@email.com");
            var dataNascimento = new DateTime(1990, 5, 15);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => 
                new Usuario("", email, dataNascimento));
        }

        [Fact]
        public void CriarUsuario_ComDataNascimentoFutura_DeveLancarExcecao()
        {
            // Arrange
            var nome = "João Silva";
            var email = Email.Create("joao@email.com");
            var dataNascimentoFutura = DateTime.Today.AddDays(1);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => 
                new Usuario(nome, email, dataNascimentoFutura));
        }

        [Fact]
        public void PodeEmprestarLivro_UsuarioAtivoSemEmprestimos_DeveRetornarTrue()
        {
            // Arrange
            var usuario = new Usuario(
                "João Silva", 
                Email.Create("joao@email.com"), 
                new DateTime(1990, 5, 15), 
                3);

            // Act
            var podeEmprestar = usuario.PodeEmprestarLivro();

            // Assert
            Assert.True(podeEmprestar);
        }

        [Fact]
        public void Desativar_UsuarioSemEmprestimosAtivos_DeveDesativarComSucesso()
        {
            // Arrange
            var usuario = new Usuario(
                "João Silva", 
                Email.Create("joao@email.com"), 
                new DateTime(1990, 5, 15));

            // Act
            usuario.Desativar();

            // Assert
            Assert.False(usuario.Ativo);
        }

        [Fact]
        public void CalcularIdade_ComDataNascimentoValida_DeveCalcularCorretamente()
        {
            // Arrange
            var dataNascimento = DateTime.Today.AddYears(-30);
            var usuario = new Usuario(
                "João Silva", 
                Email.Create("joao@email.com"), 
                dataNascimento);

            // Act
            var idade = usuario.CalcularIdade();

            // Assert
            Assert.Equal(30, idade);
        }

        [Fact]
        public void AtualizarNome_ComNomeValido_DeveAtualizarComSucesso()
        {
            // Arrange
            var usuario = new Usuario(
                "João Silva", 
                Email.Create("joao@email.com"), 
                new DateTime(1990, 5, 15));
            var novoNome = "João Pedro Silva";

            // Act
            usuario.AtualizarNome(novoNome);

            // Assert
            Assert.Equal(novoNome, usuario.Nome);
            Assert.NotNull(usuario.UpdatedAt);
        }
    }
}
