using System;
using System.Text.RegularExpressions;

namespace MonomParse
{
    internal class Monome
    {
        private string expression;
        private string variable;
        private int? coefficient;
        private int? exponent;
        public IExpressionParser parser;

        public Monome(string expression, IExpressionParser parser)
        {
            this.parser = parser;
            this.ParseExpression(expression);
        }

        public Monome(int? coefficient, string variable, int? exponent, IExpressionParser parser)
        {
            this.parser = parser;
            this.coefficient = coefficient;
            this.variable = variable?.Trim();
            this.exponent = exponent;
            this.expression = parser.MakeStringExpression(this.coefficient, this.variable, this.exponent);
        }

        private void ParseExpression(string fromExpression)
        {
            this.expression = fromExpression;
            this.variable = parser.ParseVariable(fromExpression);
            this.coefficient = parser.ParseCoefficient(fromExpression);
            this.exponent = parser.ParseExponent(fromExpression);
        }

        public string Variable => this.variable;
        public int? Coefficient => this.coefficient;
        public int? Exponent => this.exponent;
        public string Expression => this.expression;

    }
}