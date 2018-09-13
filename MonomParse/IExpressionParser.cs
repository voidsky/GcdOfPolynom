using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonomParse
{
    interface IExpressionParser
    {
        int ParseExponent(string expression);
        int ParseCoefficient(string expression);
        string ParseVariable(string expression);
        string MakeStringExpression(int? coefficient, string variable, int? exponent);
    }
}
