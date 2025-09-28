using System.Data;
using Microsoft.AspNetCore.Mvc;

namespace Day02_Routing
{
    public class MonthRouteConstraint : IRouteConstraint
    {
        

        public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {

            if (!values.TryGetValue(routeKey, out var routeValue)) return false;

            if (int.TryParse(routeValue?.ToString(), out var month))
                return month >= 1 && month < 31;

            return false;
        }
    }
}
