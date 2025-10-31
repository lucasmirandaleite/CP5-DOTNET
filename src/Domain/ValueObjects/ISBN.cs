using System;
using System.Text.RegularExpressions;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.ValueObjects
{
    public class ISBN : IEquatable<ISBN>
    {
        private static readonly Regex IsbnRegex = new Regex(
            @"^(?:ISBN(?:-1[03])?:? )?(?=[0-9X]{10}$|(?=(?:[0-9]+[- ]){3})[- 0-9X]{13}$|97[89][0-9]{10}$|(?=(?:[0-9]+[- ]){4})[- 0-9]{17}$)(?:97[89][- ]?)?[0-9]{1,5}[- ]?[0-9]+[- ]?[0-9]+[- ]?[0-9X]$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        [BsonElement("value")]
        public string Value { get; }

        private ISBN(string value)
        {
            Value = value;
        }

        public static ISBN Create(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn))
                throw new ArgumentException("ISBN não pode ser vazio", nameof(isbn));

            isbn = isbn.Trim().Replace("-", "").Replace(" ", "").ToUpperInvariant();

            if (!IsValidISBN(isbn))
                throw new ArgumentException("Formato de ISBN inválido", nameof(isbn));

            return new ISBN(isbn);
        }

        private static bool IsValidISBN(string isbn)
        {
            if (isbn.Length == 10)
                return IsValidISBN10(isbn);
            
            if (isbn.Length == 13)
                return IsValidISBN13(isbn);

            return false;
        }

        private static bool IsValidISBN10(string isbn)
        {
            int sum = 0;
            for (int i = 0; i < 9; i++)
            {
                if (!char.IsDigit(isbn[i]))
                    return false;
                sum += (isbn[i] - '0') * (10 - i);
            }

            char checkDigit = isbn[9];
            if (checkDigit == 'X')
                sum += 10;
            else if (char.IsDigit(checkDigit))
                sum += checkDigit - '0';
            else
                return false;

            return sum % 11 == 0;
        }

        private static bool IsValidISBN13(string isbn)
        {
            int sum = 0;
            for (int i = 0; i < 12; i++)
            {
                if (!char.IsDigit(isbn[i]))
                    return false;
                sum += (isbn[i] - '0') * (i % 2 == 0 ? 1 : 3);
            }

            if (!char.IsDigit(isbn[12]))
                return false;

            int checkDigit = isbn[12] - '0';
            int calculatedCheckDigit = (10 - (sum % 10)) % 10;

            return checkDigit == calculatedCheckDigit;
        }

        public bool Equals(ISBN other)
        {
            if (other is null) return false;
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ISBN);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value;
        }

        public static implicit operator string(ISBN isbn)
        {
            return isbn?.Value;
        }

        public static bool operator ==(ISBN left, ISBN right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ISBN left, ISBN right)
        {
            return !Equals(left, right);
        }
    }
}

