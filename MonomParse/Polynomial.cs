using System;
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

        public List<Monomial> Monomials { get; set;  }
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

        public void SortAndFillWithMissing()
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
            this.Monomials = polyWithMissing.Monomials;
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

        public Polynomial Divide(Polynomial n, Polynomial d, out Polynomial reminder)
        {
            n.SortAndFillWithMissing();
            d.SortAndFillWithMissing();

            Polynomial q = new Polynomial("", Parser);
            Polynomial r = n;

            while (!r.IsZero() && r.Degree() >= d.Degree())
            {
                Monomial lead_r = r.GetWithHighestDegree();
                Monomial lead_d = d.GetWithHighestDegree();
                Monomial t = lead_r.DivideMonomialWithSameVariable(lead_d);
                q.AddMonomial(t);
                var multiplied = d.MultiplyBy(t);
                r = r.Subtract(multiplied);
                r.RemoveZeros();
                r.SortAndFillWithMissing();
            }

            reminder = r;
            return q;
        }

        private void RemoveZeros()
        {
            List<Monomial> temp = new List<Monomial>();
            foreach (var monom in Monomials)
            {
                if (monom.Coefficient != 0)
                {
                    temp.Add(monom);
                }
            }
            this.Monomials = temp;
        }

        private bool IsZero()
        {
            decimal coefficientSum = 0;
            foreach (var mon in Monomials)
            {
                coefficientSum += mon.Coefficient;
            }

            return coefficientSum == 0;
        }

        public Polynomial Subtract(Polynomial substractBy)
        {
            if (this.Degree() != substractBy.Degree())
                throw new InvalidMonomialOperationException();

            Polynomial result = new Polynomial("", Parser);
            Monomial[] fromMonoms = this.Monomials.ToArray();
            Monomial[] subtractMonoms = substractBy.Monomials.ToArray();

            for (int x = 0; x < this.MonomialCount(); x++)
            {
                Monomial subtractResult;
                if (x < substractBy.MonomialCount())
                {
                    subtractResult = fromMonoms[x].SubtractMonomialWithSameVariable(subtractMonoms[x]);
                }
                else
                {
                    subtractResult = fromMonoms[x];
                }

                result.AddMonomial(subtractResult);
            }

            return result;
        }

        public Polynomial MultiplyBy(Monomial mult)
        {
            if (mult == null) throw new ArgumentNullException(nameof(mult));
            Polynomial newPoly = new Polynomial("",Parser);
            foreach (Monomial monomial in Monomials)
            {
                newPoly.AddMonomial(monomial.MultiplyBy(mult));
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