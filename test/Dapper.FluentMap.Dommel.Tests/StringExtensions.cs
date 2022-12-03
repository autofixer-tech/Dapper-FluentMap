using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Dapper.FluentMap.Dommel.Tests
{
    public static class StringExtensions
    {
        public static string ToCamelCase(this string @this)
        {
            if (string.IsNullOrEmpty(@this) || @this.Length <= 1)
            {
                return @this;
            }

            return char.ToLowerInvariant(@this[0]) + @this.Substring(1);
        }

        public static string ToSnakeCase(this string @this)
        {
            if (string.IsNullOrEmpty(@this))
            {
                return @this;
            }

            return string.Concat(@this.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x : x.ToString()))
                .ToLowerInvariant();
        }

        public static string ToPrivateBackingField(this string @this)
        {
            if (string.IsNullOrEmpty(@this))
            {
                return @this;
            }

            return $"_{@this.ToCamelCase()}";
        }

        public static string TrimInterpolated(this string @this)
        {
            if (string.IsNullOrEmpty(@this))
            {
                return @this;
            }

            return @this.Replace("'", string.Empty).Replace("{", "[").Trim().Replace("}", "]").Trim();
        }

        public static string TrimSql(this string @this)
        {
            if (string.IsNullOrEmpty(@this))
            {
                return @this;
            }

            return @this.Trim().Trim(',');
        }

        public static decimal ToDecimal(this string @this)
        {
            if (string.IsNullOrEmpty(@this))
            {
                throw new ArgumentNullException(nameof(@this));
            }

            return decimal.Parse(@this.Replace('.', ','));
        }

        public static string RemoveDiacritics(this string @this)
        {
            var normalizedString = @this.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder(normalizedString.Length);

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder
                .ToString()
                .Normalize(NormalizationForm.FormC);
        }

        public static string ToHtmlString(this string @this)
        {
            if (string.IsNullOrEmpty(@this))
            {
                return null;
            }

            return @this
                .Replace("&", "&amp;")
                .Replace("<", "lt;")
                .Replace(">", "gt;");
        }

        public static string ToHtmlString(this StringBuilder @this)
        {
            if (@this == null)
            {
                throw new ArgumentNullException(nameof(@this));
            }

            return @this.ToString()
                .Replace("&", "&amp;")
                .Replace("&amp;amp;", "&amp;");
        }

        public static string FirstCharToUpper(this string input) =>
            input switch
            {
                null => throw new ArgumentNullException(nameof(input)),
#pragma warning disable SA1122
                "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
#pragma warning restore SA1122
                _ => input[0].ToString().ToUpper() + input[1..]
            };
    }
}