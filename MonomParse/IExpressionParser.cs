using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonomialParse
{
    public interface IExpressionParser
    {
        int? ExtractExponent(string expression);
        int ExtractCoefficient(string expression);
        string ExtractVariable(string expression);
        string CombineStringExpression(decimal? coefficient, string variable, int? exponent);

    }
}
