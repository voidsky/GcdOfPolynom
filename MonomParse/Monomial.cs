using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace MonomialParse
{
    public class Monomial : ICloneable
    {
        private string expression;
        private string variable;
        private decimal coefficient;
        private int? exponent;
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
            this.exponent = exponent;
            this.expression = parser.CombineStringExpression(this.coefficient, this.variable, this.exponent);
        }

        public Monomial(Monomial from)
        {
            this.parser = from.parser;
            this.coefficient = from.coefficient;
            this.variable = from.variable;
            this.exponent = from.exponent;
            this.expression = parser.CombineStringExpression(this.coefficient, this.variable, this.exponent);
        }


        private void ParseExpression(string fromExpression)
        {
            this.expression = fromExpression;
            this.variable = parser.ExtractVariable(fromExpression);
            this.coefficient = parser.ExtractCoefficient(fromExpression);
            if (variable.Length > 0)
            {
                this.exponent = parser.ExtractExponent(fromExpression);
            }
            else
            {
                this.exponent = null;
            }
        }

        public Monomial AddMonomialWithSameVariable(Monomial monomialToAdd)
        {
            if (!IsVariableAndExponentSame(monomialToAdd)) throw new InvalidMonomialOperationException();
            var newCoefficient = this.coefficient + monomialToAdd.coefficient;
            return new Monomial(newCoefficient, this.variable, this.exponent, parser);
        }

        public Monomial Subtract(Monomial that)
        {
            var newCoefficient = this.coefficient - that.coefficient;
            return new Monomial(newCoefficient, this.variable, this.exponent, parser);
        }

        public Monomial DivideMonomialWithSameVariable(Monomial divisorMonomial)
        {
            if (!divisorMonomial.HasVariable() && divisorMonomial.coefficient == 1) return this;
            //if (!IsVariableSame(divisorMonomial)) throw new InvalidMonomialOperationException();
            if (divisorMonomial.coefficient == 0) throw new DivideByZeroException();

            var newCoefficient = decimal.Round(this.coefficient / divisorMonomial.coefficient,3);
            int? newExponent = null;
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
        public decimal Coefficient => this.coefficient;
        public int? Exponent => this.exponent;

        public int Degree()
        {
            if (this.HasVariable() && Exponent != null)
            {
                return (int)exponent;
            } 
            else if (this.coefficient > 0)
            {
                return 0;
            }

            return 0;
        }

        public string Expression
        {
            get { return expression;  }
        }
        
       public int CompareTo(Monomial that)
       {
           if (this.variable?.Length == 0 && that.variable?.Length == 0) return 0;
            if (this.exponent > that.exponent || that.exponent == null) return -1;
            if (this.exponent < that.exponent || this.exponent == null) return 1;

            if (this.exponent == that.exponent)
            {
                if (this.coefficient > that.coefficient) return -1;
                if (this.coefficient < that.coefficient) return 1;
                return 0;
            }

            return 1;
        }

        public Monomial MultiplyBy(Monomial mult)
        {
            var newCoefficient = Decimal.Round(this.coefficient * mult.coefficient);
            int? newExponent = (this.exponent ?? 0) + (mult.exponent ?? 0);

            string newVariable = "";
            if (this.variable == mult.variable)
            {
                newVariable = this.variable;
            }
            else if (!this.HasVariable() && mult.HasVariable())
            {
                newVariable = mult.variable;

            }
            else
            {
                newVariable = this.variable;

            }

            return new Monomial(newCoefficient, newVariable, newExponent, parser);
        }

        public bool HasVariable()
        {
            return (this.variable != null) && (variable.Length > 0);
        }

        public object Clone()
        {
            Monomial clone = new Monomial(this.coefficient,
                this.variable,
                this.exponent,
                this.parser);
            return clone;
        }
    }
}