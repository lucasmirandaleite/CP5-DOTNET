using System;
using Xunit;
using Domain.ValueObjects;

namespace Domain.Tests
{
    public class ISBNTests
    {
        [Theory]
        [InlineData("9780134494166")] // ISBN-13 válido
        [InlineData("0134494164")] // ISBN-10 válido
        [InlineData("978-0-13-449416-6")] // ISBN-13 com hífens
        [InlineData("0-13-449416-4")] // ISBN-10 com hífens
        public void Create_ComISBNValido_DeveCriarComSucesso(string isbnValido)
        {
            // Act
            var isbn = ISBN.Create(isbnValido);

            // Assert
            Assert.NotNull(isbn);
            Assert.NotNull(isbn.Value);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Create_ComISBNVazio_DeveLancarExcecao(string isbnInvalido)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => ISBN.Create(isbnInvalido));
        }

        [Theory]
        [InlineData("123")] // Muito curto
        [InlineData("12345678901234567890")] // Muito longo
        [InlineData("9780134494167")] // ISBN-13 com dígito verificador inválido
        public void Create_ComFormatoInvalido_DeveLancarExcecao(string isbnInvalido)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => ISBN.Create(isbnInvalido));
        }

        [Fact]
        public void Equals_ISBNsIguais_DeveRetornarTrue()
        {
            // Arrange
            var isbn1 = ISBN.Create("9780134494166");
            var isbn2 = ISBN.Create("978-0-13-449416-6");

            // Act & Assert
            Assert.True(isbn1.Equals(isbn2));
            Assert.True(isbn1 == isbn2);
        }

        [Fact]
        public void Equals_ISBNsDiferentes_DeveRetornarFalse()
        {
            // Arrange
            var isbn1 = ISBN.Create("9780134494166");
            var isbn2 = ISBN.Create("9780134685991");

            // Act & Assert
            Assert.False(isbn1.Equals(isbn2));
            Assert.True(isbn1 != isbn2);
        }

        [Fact]
        public void ToString_DeveRetornarValorDoISBN()
        {
            // Arrange
            var isbn = ISBN.Create("9780134494166");

            // Act
            var resultado = isbn.ToString();

            // Assert
            Assert.Equal("9780134494166", resultado);
        }

        [Fact]
        public void ImplicitConversion_DeveConverterParaString()
        {
            // Arrange
            var isbn = ISBN.Create("9780134494166");

            // Act
            string isbnString = isbn;

            // Assert
            Assert.Equal("9780134494166", isbnString);
        }
    }
}
