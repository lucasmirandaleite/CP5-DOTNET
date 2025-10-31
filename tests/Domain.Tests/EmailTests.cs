using System;
using Xunit;
using Domain.ValueObjects;

namespace Domain.Tests
{
    public class EmailTests
    {
        [Theory]
        [InlineData("teste@email.com")]
        [InlineData("usuario.teste@dominio.com.br")]
        [InlineData("user123@test.org")]
        public void Create_ComEmailValido_DeveCriarComSucesso(string emailValido)
        {
            // Act
            var email = Email.Create(emailValido);

            // Assert
            Assert.NotNull(email);
            Assert.Equal(emailValido.ToLowerInvariant(), email.Value);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Create_ComEmailVazio_DeveLancarExcecao(string emailInvalido)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => Email.Create(emailInvalido));
        }

        [Theory]
        [InlineData("emailsemarroba.com")]
        [InlineData("@dominio.com")]
        [InlineData("usuario@")]
        [InlineData("usuario @dominio.com")]
        public void Create_ComFormatoInvalido_DeveLancarExcecao(string emailInvalido)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => Email.Create(emailInvalido));
        }

        [Fact]
        public void Equals_EmailsIguais_DeveRetornarTrue()
        {
            // Arrange
            var email1 = Email.Create("teste@email.com");
            var email2 = Email.Create("TESTE@EMAIL.COM");

            // Act & Assert
            Assert.True(email1.Equals(email2));
            Assert.True(email1 == email2);
        }

        [Fact]
        public void Equals_EmailsDiferentes_DeveRetornarFalse()
        {
            // Arrange
            var email1 = Email.Create("teste1@email.com");
            var email2 = Email.Create("teste2@email.com");

            // Act & Assert
            Assert.False(email1.Equals(email2));
            Assert.True(email1 != email2);
        }

        [Fact]
        public void ToString_DeveRetornarValorDoEmail()
        {
            // Arrange
            var emailString = "teste@email.com";
            var email = Email.Create(emailString);

            // Act
            var resultado = email.ToString();

            // Assert
            Assert.Equal(emailString, resultado);
        }

        [Fact]
        public void ImplicitConversion_DeveConverterParaString()
        {
            // Arrange
            var email = Email.Create("teste@email.com");

            // Act
            string emailString = email;

            // Assert
            Assert.Equal("teste@email.com", emailString);
        }
    }
}
