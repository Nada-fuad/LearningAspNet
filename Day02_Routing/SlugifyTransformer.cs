using Microsoft.AspNetCore.Routing;
using System.Text.RegularExpressions;

namespace Day02_Routing
{
    // "My Book Is Big" -> "my-book-is-big"
    public class SlugifyTransformer : IOutboundParameterTransformer
    {
        public string? TransformOutbound(object? value)
        {
            if (value == null)
                return null;

            var str = value.ToString();

            if (string.IsNullOrEmpty(str))
                return null;

            str = str.ToLowerInvariant();

            str = Regex.Replace(str, @"\s+", "-"); 
            str = Regex.Replace(str, @"[^a-z0-9\-]", "");

            str = Regex.Replace(str, "-{2,}", "-").Trim('-');

            return str;
        }
    }
}
