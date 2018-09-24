using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonomialParse;

namespace MonomParse
{
    public class Polynomial
    {
        private List<Monomial> monomials;

        public Polynomial(string expression, IExpressionParser parser)
        {
            monomials = new List<Monomial>();
            var monomialStrings = new MonomialStrings(expression);
            foreach (var monomialString in monomialStrings)
            {
                this.monomials.Add(new Monomial(monomialString,parser));
            }
        }

        public int MonomialCount()
        {
            return this.monomials.Count();
        }

        public void ArrangeDescending()
        {
            //monomials.Sort((x,y)=>x.Exponent.co);
        }
    }
}
