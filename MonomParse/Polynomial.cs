using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonomialParse;

namespace MonomParse
{
    public class Polynomial : ICloneable, IEquatable<Polynomial>
    {
        public Polynomial(string expression, IExpressionParser parser)
        {
            Parser = parser;
            Monomials = new List<Monomial>();
            var monomialStrings = new MonomialStrings(expression);

            foreach (var monomialString in monomialStrings)
                Monomials.Add(new Monomial(monomialString, parser));
        }

        public List<Monomial> Monomials { get; set; }
        private IExpressionParser Parser { get; }

        public object Clone()
        {
            var clone = new Polynomial(null, Parser);
            foreach (var monomial in Monomials) clone.AddMonomial((Monomial) monomial.Clone());
            return clone;
        }

        public bool Equals(Polynomial other)
        {
            return Expression().Equals(other.Expression());
        }

        public int Count()
        {
            return Monomials.Count();
        }

        public void SortDescending()
        {
            Monomials.Sort((x, y) => x.CompareTo(y));
        }

        private void AddMonomial(Monomial monom)
        {
            Monomials.Add(monom);
        }

        public string Expression()
        {
            var str = new StringBuilder();
            foreach (var monomial in Monomials)
            {
                if (str.Length > 0 && monomial.Coefficient >= 0) str.Append("+");
                str.Append(monomial.Expression);
            }

            return str.ToString();
        }

        private Polynomial FillWithMissing(int from, string varName)
        {
            var filled = new Polynomial(null, Parser);

            for (var x = from; x >= 1; x--)
            {
                var testing = GetWithExponent(x);

                if (testing != null)
                    filled.AddMonomial(new Monomial(testing));
                else
                    filled.AddMonomial(new Monomial(0, varName, x, Parser));
            }

            filled.AddMonomial(new Monomial(SumFreeCoeficients(), null, null, Parser));

            return filled;
        }

        private Monomial GetWithExponent(int i)
        {
            foreach (var monom in Monomials)
                if (monom.HasVariable() && monom.Exponent == i)
                    return monom;
            return null;
        }

        private decimal SumFreeCoeficients()
        {
            decimal sum = 0;
            foreach (var monom in Monomials)
                if (!monom.HasVariable())
                    sum += monom.Coefficient;
            return sum;
        }

        public int? Degree()
        {
            int? maxDegree = null;
            foreach (var monomial in Monomials)
                if (monomial.HasVariable())
                {
                    if (monomial.Exponent > maxDegree || maxDegree == null)
                        maxDegree = monomial.Exponent;
                }
                else
                {
                    if (1 > maxDegree || maxDegree == null)
                        maxDegree = 0;

                }

            return maxDegree;
        }

        private void RemoveZeroCoefMonoms()
        {
            var temp = new List<Monomial>();
            foreach (var monom in Monomials)
                if (monom.Coefficient != 0)
                    temp.Add(monom);
            Monomials = temp;
        }

        private bool IsZero()
        {
            decimal coefficientSum = 0;
            foreach (var mon in Monomials)
                if (mon.Coefficient != 0)
                    coefficientSum = mon.Coefficient;
            return coefficientSum == 0;
        }

        private int? MaxDegree(Polynomial other)
        {
            var maxDegree = Degree();
            if (other.Degree() > Degree()) maxDegree = other.Degree();
            if (maxDegree == null) maxDegree = 0;
            return maxDegree;
        }

        public Polynomial Subtract(Polynomial other)
        {
            var result = new Polynomial(null, Parser);
            var maxDegree = MaxDegree(other);

            var fromTemp = FillWithMissing((int) maxDegree, FirstVariableName());
            var substrTemp = other.FillWithMissing((int) maxDegree, FirstVariableName());
            fromTemp.SortDescending();
            substrTemp.SortDescending();

            using (var e1 = fromTemp.Monomials.GetEnumerator())
            using (var e2 = substrTemp.Monomials.GetEnumerator())
            {
                while (e1.MoveNext() && e2.MoveNext())
                {
                    var item1 = e1.Current;
                    var item2 = e2.Current;
                    var subtractResult = item1.Subtract(item2);
                    result.AddMonomial(subtractResult);
                }
            }

            result.RemoveZeroCoefMonoms();
            return result;
        }

        /* Exercise 16. */
        public Polynomial Divide(Polynomial d, out Polynomial reminder)
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
            
            var n = (Polynomial) Clone();

            n.SortDescending();
            n = n.FillWithMissing(n.Degree() ?? 0, n.FirstVariableName());
            d.SortDescending();
            d = d.FillWithMissing(d.Degree() ?? 0, d.FirstVariableName());

            var q = new Polynomial("", Parser);
            var r = n;

            while (!r.IsZero() && r.Degree() >= d.Degree())
            {
                var lead_r = r.GetWithHighestDegree();
                var lead_d = d.GetWithHighestDegree();
                var t = lead_r.DivideMonomialWithSameVariable(lead_d);
                q.AddMonomial(t);
                var multiplied = d.MultiplyBy(t);
                r = r.Subtract(multiplied);
                r.RemoveZeroCoefMonoms();
                r.SortDescending();
                r = r.FillWithMissing(r.Degree() ?? 0, r.FirstVariableName());
            }

            reminder = r;
            return q;
        }

        private string FirstVariableName()
        {
            foreach (var monom in Monomials)
                if (monom.HasVariable())
                    return monom.Variable;
            return null;
        }

        public Polynomial MultiplyBy(Monomial mult)
        {
            if (mult == null) throw new ArgumentNullException(nameof(mult));
            var newPoly = new Polynomial("", Parser);
            foreach (var monomial in Monomials) newPoly.AddMonomial(monomial.MultiplyBy(mult));
            return newPoly;
        }

        public Monomial GetWithHighestDegree()
        {
            var degree = Degree();
            foreach (var monomial in Monomials)
                if (monomial.Degree() == degree)
                    return monomial;
            return null;
        }

        /* Exercise 1 */
        public Polynomial Gcd(Polynomial other)
        {
            var a = (Polynomial)this.Clone();
            var b = (Polynomial)other.Clone();

            if (b.IsZero())
            {
                return a;
            }
            else
            {
                var x = a.Divide(b, out var r);
                var c = b.Gcd(r);
                return c;
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Polynomial) obj);
        }

        public override int GetHashCode()
        {
            return Monomials != null ? Monomials.GetHashCode() : 0;
        }
    }
}