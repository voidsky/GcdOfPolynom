using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonomialParse;

namespace MonomParse
{
    public class Polynomial
    {
        public List<Monomial> Monomials { get; }
        private IExpressionParser Parser { get; set; }

        public Polynomial(string expression, IExpressionParser parser)
        {
            this.Parser = parser;
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
            this.Monomials.Add(monom);
        }

        public string PolynomialString()
        {
            StringBuilder str = new StringBuilder();
            foreach (var monomial in Monomials)
            {
                if (str.Length>0 && monomial.Coefficient >= 0) str.Append("+");
                str.Append(monomial.Expression);
            }
            return str.ToString();
        }

        public Polynomial SortedAndFilledWithMissing()
        {
            int? exponent;
            int? lastExponent = null;
            string variableName = "";
            Polynomial polyWithMissing = new Polynomial("", this.Parser);
            SortDescending();
            foreach (Monomial monom in Monomials)
            {
                if (monom.Variable == null) continue;
                variableName = monom.Variable;

                exponent = monom.Exponent;
                if (lastExponent != null && lastExponent-1 != exponent)
                {
                    lastExponent = AddExponentRange(polyWithMissing, lastExponent-1, exponent, variableName);
                }
                polyWithMissing.AddMonomial(monom);
                lastExponent = exponent;
            }

            lastExponent = AddExponentRange(polyWithMissing, lastExponent-1, 0, variableName);
            return polyWithMissing;
        }

        private int? AddExponentRange(Polynomial toPolynomial, int? fromExponent, int? toExponent, string variableName)
        {
            while (toExponent < fromExponent)
            {
                var dummyMonom = new Monomial(0, variableName, fromExponent, this.Parser);
                toPolynomial.AddMonomial(dummyMonom);
                fromExponent--;
            }
            return fromExponent;
        }
    }
}