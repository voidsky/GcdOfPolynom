using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using MonomialParse;

namespace MonomParse
{
    public class Polynomial
    {
        public Polynomial(string expression, IExpressionParser parser)
        {
            Parser = parser;
            Monomials = new List<Monomial>();
            var monomialStrings = new MonomialStrings(expression);
            foreach (var monomialString in monomialStrings)
                Monomials.Add(new Monomial(monomialString, parser));
        }

        public List<Monomial> Monomials { get; }
        private IExpressionParser Parser { get; }

        public int MonomialCount()
        {
            return Monomials.Count();
        }

        public void SortDescending()
        {
            Monomials.Sort((x, y) => x.CompareTo(y));
        }

        public void AddMonomial(Monomial monom)
        {
            Monomials.Add(monom);
        }

        public string PolynomialString()
        {
            var str = new StringBuilder();
            foreach (var monomial in Monomials)
            {
                if (str.Length > 0 && monomial.Coefficient >= 0) str.Append("+");
                str.Append(monomial.Expression);
            }

            return str.ToString();
        }

        public Polynomial SortedAndFilledWithMissing()
        {
            int? lastExponent = null;
            var variableName = "";
            var polyWithMissing = new Polynomial("", Parser);
            SortDescending();
            foreach (var monom in Monomials)
            {
                if (monom.Variable == null) continue;
                if (variableName.Length == 0) variableName = monom.Variable;

                var exponent = monom.Exponent;
                var expectedExponent = lastExponent - 1;

                if (lastExponent != null && expectedExponent != exponent)
                    AddExponentRange(polyWithMissing, expectedExponent, exponent??0, variableName);
                polyWithMissing.AddMonomial(monom);
                lastExponent = exponent;
            }

            AddExponentRange(polyWithMissing, lastExponent - 1, 0, variableName);
            return polyWithMissing;
        }

        private void AddExponentRange(Polynomial toPolynomial, int? fromExponent, int? toExponent, string variableName)
        {
            while (toExponent < fromExponent)
            {
                var dummyMonom = new Monomial(0, variableName, fromExponent, Parser);
                toPolynomial.AddMonomial(dummyMonom);
                fromExponent--;
            }
        }

        public int? Degree()
        {
            int? maxDegree = null;
            foreach (Monomial monomial in Monomials)
            {
                if (monomial.Variable.Length > 0 &&
                    monomial.Coefficient != 0)
                {
                    if (monomial.Exponent > maxDegree || maxDegree == null)
                        maxDegree = monomial.Exponent;
                }
            }

            return maxDegree;
        }

        public void Divide(Polynomial divideWhat, Polynomial divideBy, Polynomial result, Polynomial reminder)
        {
            result = new Polynomial("", Parser);
            divideWhat = divideWhat.SortedAndFilledWithMissing();
            divideBy = divideBy.SortedAndFilledWithMissing();

            Monomial highestMonomial = divideWhat.GetWithHighestDegree();
            Monomial byHighest = divideBy?.GetWithHighestDegree();
            Monomial divideResult = highestMonomial.DivideMonomialWithSameVariable(byHighest);
            Polynomial multiplicationResult = divideBy.MultiplyBy(divideResult);
            Polynomial subtractResult = divideWhat.Subtract(multiplicationResult);
            result.AddMonomial(divideResult);

            while (subtractResult.Degree() > divideBy.Degree())
            {
                divideResult = subtractResult.GetWithHighestDegree()?.
                    DivideMonomialWithSameVariable(divideBy?.GetWithHighestDegree());
                multiplicationResult = divideBy.MultiplyBy(divideResult);
                subtractResult = subtractResult.Subtract(multiplicationResult);
                result.AddMonomial(divideResult);
            }

        }

        public Polynomial Subtract(Polynomial substractBy)
        {
            if (this.Degree() != substractBy.Degree())
                throw new InvalidMonomialOperationException();
            if (this.MonomialCount() != substractBy.MonomialCount())
                throw new InvalidMonomialOperationException();

            Polynomial result = new Polynomial("", Parser);
            Monomial[] fromMonoms = this.Monomials.ToArray();
            Monomial[] subtractMonoms = substractBy.Monomials.ToArray();

            for (int x = 0; x < this.MonomialCount(); x++)
            {
                Monomial subtractResult = fromMonoms[x].SubtractMonomialWithSameVariable(subtractMonoms[x]);
                result.AddMonomial(subtractResult);
            }
            return result;
        }

        public Polynomial MultiplyBy(Monomial divideResult)
        {
            Polynomial newPoly = new Polynomial("",Parser);
            foreach (Monomial monomial in Monomials)
            {
                newPoly.AddMonomial(monomial.MultiplyBy(divideResult));
            }
            return newPoly;
        }

        public Monomial GetWithHighestDegree()
        {
            int? degree = this.Degree();
            foreach (Monomial monomial in Monomials)
            {
                if (monomial.Exponent == degree) return monomial;
            }

            return null;
        }
    }
}