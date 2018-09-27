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
    public class Polynomial : ICloneable, IEquatable<Polynomial>
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

        public int Count()
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

        public Polynomial FillWithMissing(int from, string varName)
        {
            Polynomial filled = new Polynomial(null, Parser);

            for (int x = from; x >= 1; x--)
            {
                Monomial testing = this.GetWithExponent(x);

                if (testing != null)
                {
                    filled.AddMonomial(new Monomial(testing));
                }
                else
                {
                    filled.AddMonomial(new Monomial(0, varName, x, Parser));
                }
            }

            filled.AddMonomial(new Monomial(this.SumFreeCoeficients(), null, null, Parser));

            return filled;
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

        public int? Degree()
        {
            int? maxDegree = null;
            foreach (Monomial monomial in Monomials)
            {
                if (monomial.HasVariable())
                {
                    if (monomial.Exponent > maxDegree || maxDegree == null)
                        maxDegree = monomial.Exponent;
                }
            }

            return maxDegree;
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

        private int? MaxDegree(Polynomial other)
        {
            int? maxDegree = this.Degree();
            if (other.Degree() > this.Degree()) maxDegree = other.Degree();
            if (maxDegree == null) maxDegree = 0;
            return maxDegree;
        }

        public Polynomial Subtract(Polynomial other)
        {
            Polynomial result = new Polynomial(null, Parser);
            int? maxDegree = MaxDegree(other);

            Polynomial fromTemp = FillWithMissing((int)maxDegree, FirstVariableName());
            Polynomial substrTemp = other.FillWithMissing((int)maxDegree, FirstVariableName());
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

        public Polynomial Divide( Polynomial d, out Polynomial reminder)
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
            Polynomial n = (Polynomial)this.Clone();
            n.SortDescending();
            n.FillWithMissing((int)n.Degree(),n.FirstVariableName());
            d.SortDescending();
            d.FillWithMissing((int)d.Degree(), d.FirstVariableName());

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
                r.SortDescending();
                r = r.FillWithMissing(r.Degree()??0, r.FirstVariableName());
            }

            reminder = r;
            return q;
        }


        private string FirstVariableName()
        {
            foreach (var monom in Monomials)
            {
                if (monom.HasVariable())
                {
                    return monom.Variable;
                }
            }
            return null;
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

        public Polynomial Gcd(Polynomial other)
        {
            Polynomial r;
            Polynomial q = this.Divide(other, out r);
            if (r.IsZero())
            {
                return q;
            }
            else
            {
                return other.Gcd(r);
            }

        }

        public object Clone()
        {
            Polynomial clone = new Polynomial(null, Parser);
            foreach (Monomial monomial in Monomials)
            {
                clone.AddMonomial((Monomial)monomial.Clone());
            }
            return clone;
        }

        public bool Equals(Polynomial other)
        {
            return this.Expression().Equals(other.Expression());
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Polynomial) obj);
        }

        public override int GetHashCode()
        {
            return (Monomials != null ? Monomials.GetHashCode() : 0);
        }
    }
}