using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using MonomialParse;

namespace MonomParse
{
    public class Polynomial
    {
        public List<Monomial> Monomials { get; set; }
        private IExpressionParser Parser { get; }


        public Polynomial(string expression, IExpressionParser parser)
        {
            Parser = parser;
            Monomials = new List<Monomial>();
            var monomialStrings = new MonomialStrings(expression);

            foreach (var monomialString in monomialStrings)
                Monomials.Add(new Monomial(monomialString, parser));
        }

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

        public void FillWithMissing(int from, string varName)
        {
            List<Monomial> listWithMissing = new List<Monomial>();

            for (int x = from; x >= 1; x--)
            {
                Monomial testing = this.GetWithExponent(x);
                if (testing != null)
                {
                    listWithMissing.Add(testing);
                }
                else
                {
                    listWithMissing.Add(new Monomial(0, varName, x, Parser));
                }
            }

            listWithMissing.Add(new Monomial(this.SumFreeCoeficients(), null, null, Parser));
            this.Monomials = listWithMissing;
        }

        private Monomial GetWithExponent(int i)
        {
            foreach (var monom in Monomials)
            {
                if (monom.HasVariable() && monom.Exponent == i)
                {
                    return monom;
                }
            }
            return null;
        }

        public decimal SumFreeCoeficients()
        {
            decimal sum = 0;
            foreach (var monom in Monomials)
            {
                if (!monom.HasVariable())
                {
                    sum += monom.Coefficient;
                }
            }
            return sum;
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
                if (monomial.HasVariable() &&
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
            /* function n / d:
                  require d ≠ 0
                  q ← 0
                  r ← n       # At each step n = d × q + r
                  while r ≠ 0 AND degree(r) ≥ degree(d):
                     t ← lead(r)/lead(d)     # Divide the leading terms
                     q ← q + t
                     r ← r − t * d
                  return (q, r)*/
             
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
                r.RemoveZeroCoefMonoms();
                r.SortAndFillWithMissing();
            }

            reminder = r;
            return q;
        }

        private void RemoveZeroCoefMonoms()
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

        public Polynomial Subtract(Polynomial that)
        {
            Polynomial result = new Polynomial("", Parser);
            int maxDegree = this.Degree()??0;
            if (that.Degree() > this.Degree()) maxDegree = that.Degree()??0;

            Polynomial fromTemp = this;
            Polynomial substrTemp = that;

            AddExponentRange(fromTemp,maxDegree,fromTemp.Degree(), fromTemp.GetWithHighestDegree().Variable);
            fromTemp.SortAndFillWithMissing();
            AddExponentRange(substrTemp, maxDegree, substrTemp.Degree(), substrTemp.GetWithHighestDegree().Variable);
            substrTemp.SortAndFillWithMissing();

            Monomial[] fromMonom = fromTemp.Monomials.ToArray();
            Monomial[] thatMonom = substrTemp.Monomials.ToArray();

            for (int x = 0; x < fromTemp.MonomialCount(); x++)
            {
                var subtractResult = fromMonom[x].Subtract(thatMonom[x]);
                result.AddMonomial(subtractResult);
            }
            result.RemoveZeroCoefMonoms();
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

        public Polynomial Gcd(Polynomial with)
        {
            Polynomial temp = new Polynomial("",Parser);
            if (this.Equals(with)) return temp = this;
            if (this.Degree() >= with.Degree()) temp = Gcd(this.Subtract(with));
            if (with.Degree() > this.Degree()) temp = Gcd(with.Subtract(this));
            return temp;
        }
    }
}