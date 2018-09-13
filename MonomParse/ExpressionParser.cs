using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MonomParse
{
    public class ExpressionParser : IExpressionParser
    {
        string expressionRegex =
            @"(?<coeficient>\s*[-]?\d+\s*?)?[\s]?"
           +@"(?<variable>\s*[A-Za-z]+\s*)?\s*"
           +@"\^?\s*(?<exponent>[-]?\d+)?";

        public int ParseExponent(string expression)
        {
            int exponent = 1;
            string stringExponent = "";
            Match exponentMatch = Regex.Match(expression ?? "", expressionRegex);
            if (exponentMatch.Success)
                stringExponent = exponentMatch.Groups["exponent"].Value;
            if (int.TryParse(stringExponent, out exponent))
                return exponent;
            return 1;
        }

        public int ParseCoefficient(string expression)
        {
            int coefficient = 1;
            string stringCoefficient = "";
            Match coefficientMatch = Regex.Match(expression ?? "", expressionRegex);
            if (coefficientMatch.Success)
                stringCoefficient = coefficientMatch.Groups["coeficient"].Value;
            if (int.TryParse(stringCoefficient, out coefficient))
                return coefficient;
            return 1;
        }

        public string ParseVariable(string expression)
        {
            string result = "";
            Match variableMatch = Regex.Match(expression ?? "", expressionRegex);
            if (variableMatch.Success)
                result = variableMatch.Groups["variable"].Value;
            result = result.Trim();
            return result;
        }

        public string MakeStringExpression(int? coefficient, string variable, int? exponent)
        {
            string expression = "";
            if (coefficient != null) expression += coefficient.ToString();
            if (variable != null && variable != "")
            {
                expression += variable;
                if (exponent != null) expression += "^" + exponent.ToString();
            }
            return expression;
        }
    }
}
