using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MonomialParse
{
    public class ExpressionParser : IExpressionParser
    {
        string expressionRegex =
            @"(?<coeficient>\s*[-]?\d*\s*?)?[\s]?"
           +@"(?<variable>\s*[A-Za-z]+\s*)?\s*"
           +@"\^?\s*(?<exponent>[-]?\d+)?";

        public int? ExtractExponent(string expression)
        {
            var stringExponent = ExtractExpressionPart(expression, "exponent");
            if (int.TryParse(stringExponent, out var exponent))
                return exponent;
            return 1;
        }

        public int ExtractCoefficient(string expression)
        {
            var stringCoefficient = ExtractExpressionPart(expression, "coeficient");
            if (stringCoefficient == "-") stringCoefficient = "-1";
                if (int.TryParse(stringCoefficient, out var coefficient))
                return coefficient;
            return 1;
        }

        public string ExtractVariable(string expression)
        {
            return ExtractExpressionPart(expression, "variable");
        }

        public string CombineStringExpression(decimal? coefficient, string variable, int? exponent)
        {
            string stringVariable = variable ?? "";
            string stringCoefficient = (coefficient ?? 1) == 1 ? "" : coefficient.ToString();
            string stringExponent = "";
            if (stringVariable.Length > 0)
            {
                stringExponent = (exponent ?? 1) == 1 ? "" : "^" + exponent.ToString();
            }
            else
            {
                stringCoefficient = (coefficient ?? 1).ToString();
            }

            string expression = $"{stringCoefficient}{stringVariable}{stringExponent}";
            return expression;
        }

        private string ExtractExpressionPart(string expression, string regexGroup)
        {
            string result = "";
            Match exponentMatch = Regex.Match(expression ?? "", expressionRegex);
            if (exponentMatch.Success)
                result = exponentMatch.Groups[regexGroup].Value.Trim();
            return result;
        }


    }
}
