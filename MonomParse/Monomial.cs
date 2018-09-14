using System;
using System.Text.RegularExpressions;

namespace MonomialParse
{
    internal class Monomial
    {
        private string expression;
        private string variable;
        private int? coefficient;
        private int? exponent;
        private IExpressionParser parser;

        public Monomial(string expression, IExpressionParser parser)
        {
            this.parser = parser;
            this.ParseExpression(expression);
        }

        public Monomial(int? coefficient, string variable, int? exponent, IExpressionParser parser)
        {
            this.parser = parser;
            this.coefficient = coefficient;
            this.variable = variable?.Trim();
            this.exponent = exponent;
            this.expression = parser.CombineStringExpression(this.coefficient, this.variable, this.exponent);
        }

        private void ParseExpression(string fromExpression)
        {
            this.expression = fromExpression;
            this.variable = parser.ExtractVariable(fromExpression);
            this.coefficient = parser.ExtractCoefficient(fromExpression);
            this.exponent = parser.ExtractExponent(fromExpression);
        }

        public Monomial AddMonomialsWithSameVariable(Monomial monomialToadd)
        {
            Monomial additionResult;
            if (IsVariableAndExponentSame(monomialToadd))
            {
                var newCoefficient = this.coefficient + monomialToadd.coefficient;
                additionResult = new Monomial(newCoefficient, this.variable, this.exponent, parser);
                return additionResult;
            }
            else
            {
                throw new MonomialsCannotBeAddedException();
            }
        }

        private bool IsVariableAndExponentSame(Monomial monomialToadd)
        {
            return this.variable == monomialToadd.variable &&
                            this.exponent == monomialToadd.exponent;
        }

        public string Variable => this.variable;
        public int? Coefficient => this.coefficient;
        public int? Exponent => this.exponent;
        public string Expression => this.expression;
    }
}