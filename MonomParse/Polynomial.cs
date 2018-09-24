using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonomialParse;

namespace MonomParse
{
    public class Polynomial
    {
        public List<Monomial> Monomials { get; }

        public Polynomial(string expression, IExpressionParser parser)
        {
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
    }
}