using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc;

namespace Day03_ModelBinding
{
    public class DataRangeQuery : IParsable<DataRangeQuery>
    {
        public DateOnly FromDate { get; set; }

        public DateOnly ToDate { get; set; }
        public static DataRangeQuery Parse(string value, IFormatProvider? provider)
        {

            if(!TryParse(value,provider, out var result))
            {
                throw new ArgumentException("could nnot parse values");
            }

            return result;
        }

        public static bool TryParse([NotNullWhen(true)] string? value, IFormatProvider? provider, [MaybeNullWhen(false)] out DataRangeQuery result)
        {
            var segments = value?.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            if(segments.Length==2 && DateOnly.TryParse(segments[0],provider,out var fromDate)&& DateOnly.TryParse(segments[1],provider,out var toDate)){
                result=new DataRangeQuery{FromDate =fromDate, ToDate=toDate};

                return true;
            }

            result = new DataRangeQuery { FromDate = default, ToDate = default};
            return false;

        }
    }
}
