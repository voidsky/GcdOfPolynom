﻿using System;
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

        public int ExtractExponent(string expression)
        {
            var stringExponent = ExtractExpressionPart(expression, "exponent");
            if (int.TryParse(stringExponent, out var exponent))
                return exponent;
            return 1;
        }

        public int ExtractCoefficient(string expression)
        {
            var stringCoefficient = ExtractExpressionPart(expression, "coeficient");
            if (int.TryParse(stringCoefficient, out var coefficient))
                return coefficient;
            return 1;
        }

        public string ExtractVariable(string expression)
        {
            return ExtractExpressionPart(expression, "variable");
        }

        public string CombineStringExpression(int? coefficient, string variable, int? exponent)
        {
            string expression = "";
            if (coefficient != null) expression += coefficient.ToString();
            if (!String.IsNullOrEmpty(variable))
            {
                expression += variable;
                if (exponent != null) expression += "^" + exponent.ToString();
            }
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
