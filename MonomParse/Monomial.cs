using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace MonomialParse
{
    public class Monomial
    {
        private string expression;
        private string variable;
        private decimal coefficient;
        private int exponent;
        private IExpressionParser parser;

        public Monomial(string expression, IExpressionParser parser)
        {
            this.parser = parser;
            this.ParseExpression(expression);
        }

        public Monomial(decimal? coefficient, string variable, int? exponent, IExpressionParser parser)
        {
            this.parser = parser;
            this.coefficient = coefficient??1;
            this.variable = variable?.Trim();
            this.exponent = exponent??1;
            this.expression = parser.CombineStringExpression(this.coefficient, this.variable, this.exponent);
        }

        private void ParseExpression(string fromExpression)
        {
            this.expression = fromExpression;
            this.variable = parser.ExtractVariable(fromExpression);
            this.coefficient = parser.ExtractCoefficient(fromExpression);
            this.exponent = parser.ExtractExponent(fromExpression);
        }

        public Monomial AddMonomialWithSameVariable(Monomial monomialToAdd)
        {
            if (!IsVariableAndExponentSame(monomialToAdd)) throw new InvalidOperationWithMonomialsException();
            var newCoefficient = this.coefficient + monomialToAdd.coefficient;
            return new Monomial(newCoefficient, this.variable, this.exponent, parser);
        }

        public Monomial SubtractMonomialWithSameVariable(Monomial monomialToSubtract)
        {
            if (!IsVariableAndExponentSame(monomialToSubtract)) throw new InvalidOperationWithMonomialsException();
            var newCoefficient = this.coefficient - monomialToSubtract.coefficient;
            return new Monomial(newCoefficient, this.variable, this.exponent, parser);
        }

        public Monomial DivideMonomialWithSameVariable(Monomial divisorMonomial)
        {
            if (!IsVariableSame(divisorMonomial)) throw new InvalidOperationWithMonomialsException();
            if (divisorMonomial.coefficient == 0) throw new DivideByZeroException();

            var newCoefficient = this.coefficient / divisorMonomial.coefficient;
            int? newExponent = 1;
            string newVariable;

            if (AreExponentsDifferent(divisorMonomial))
            {
                newExponent = this.exponent - divisorMonomial.exponent;
                newVariable = this.variable;
            }
            else
            {
                newVariable = "";
            }

            if (newCoefficient == 0)
            {
                newVariable = "";
            }

            return new Monomial(newCoefficient, newVariable, newExponent, parser);
        }


        private bool IsVariableAndExponentSame(Monomial monomial)
        {
            return this.variable == monomial.variable &&
                            this.exponent == monomial.exponent;
        }

        private bool IsVariableSame(Monomial monomial)
        {
            return this.variable == monomial.variable;
        }

        private bool AreExponentsDifferent(Monomial monomial)
        {
            return this.exponent != monomial.exponent;
        }

        public string Variable => this.variable;
        public decimal? Coefficient => this.coefficient;
        public int? Exponent => this.exponent;

        public string Expression
        {
            get { return expression;  }
        }
        
       public int CompareTo(Monomial that)
        {
            if (this.exponent > that.exponent) return -1;
            if (this.exponent == that.exponent)
            {
                if (this.coefficient > that.coefficient) return -1;
                if (this.coefficient < that.coefficient) return 1;
                return 0;
            }
            return 1;
        }

        public Monomial MultiplyBy(Monomial multiplicator)
        {
            if (!IsVariableSame(multiplicator)) throw new InvalidOperationWithMonomialsException();
            var newCoefficient = this.coefficient * multiplicator.coefficient;
            int? newExponent = this.exponent + multiplicator.exponent;
            string newVariable = this.variable;

            return new Monomial(newCoefficient, newVariable, newExponent, parser);
        }
    }
}