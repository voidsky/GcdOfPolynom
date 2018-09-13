using System;
using System.Text.RegularExpressions;

namespace MonomParse
{
    internal class Monome
    {
        private string expression;
        private string variable;
        private int coefficient;
        private int exponent;

        public Monome(string expression)
        {
            this.expression = expression;
            this.ParseExpression(this.expression);
        }

        private void ParseExpression(string expression)
        {
            this.variable = ParseVariable(expression);
            this.coefficient = ParseCoefficient(expression);
            this.exponent = ParseExponent(expression);

        }

        private int ParseExponent(string expression)
        {
            int exponent = 1;
            string stringExponent = "";
            Match exponentMatch = Regex.Match(expression ?? ""
                , @"(?<coeficient>[-]?\d+)?(?<variable>[A-Za-z]+)?\^?(?<exponent>\d+)?");
            if (exponentMatch.Success)
                stringExponent = exponentMatch.Groups["exponent"].Value;
            if (int.TryParse(stringExponent, out exponent))
                return exponent;
            return 1;
        }

        private int ParseCoefficient(string expression)
        {
            int coefficient = 1;
            string stringCoefficient = "";
            Match coefficientMatch = Regex.Match(expression ?? "", @"[-+]?\d+");
            if (coefficientMatch.Success)
                stringCoefficient = coefficientMatch.Value;
            if (int.TryParse(stringCoefficient, out coefficient))
                return coefficient;
            return 1;
        }

        private string ParseVariable(string expression)
        {
            string result = "";
            Match variableMatch = Regex.Match(expression ?? "", @"[A-Za-z]+");
            if (variableMatch.Success)
                result = variableMatch.Value;
            return result;
        }


        public string Variable {
            get {
                return this.variable;
            }
        }

        public int Coefficient {
            get {
                return this.coefficient;
            }
        }

        public int Exponent
        {
            get
            {
                return this.exponent;
            }
        }
    }
}