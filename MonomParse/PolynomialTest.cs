using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using MonomialParse;
using NUnit.Framework;

namespace MonomParse
{
    [TestFixture]
    public class PolynomialTest
    {
        [TestCase(null, 0)]
        [TestCase("", 0)]
        [TestCase("1", 1)]
        [TestCase("2x^2+1",2)]
        [TestCase("2x^3-x^1-5", 3)]
        public void CreatePolynomialResultsInCount(string polyExprtession, int count)
        {
            ExpressionParser parser = new ExpressionParser();
            Polynomial poly = new Polynomial(polyExprtession, parser);
            Assert.AreEqual(count, poly.MonomialCount());
        }

        [TestCase("2^x2+x+1","2x^2","x","1")]
        [TestCase("-x^3-x-1","-x^3","-x","-1")]

        public void CreatePolynomialResultsInMonomials(string polyExpression,
            string firstMonomExpression,
            string secondMonomExpression,
            string thirdMonomExpression)
        {
            ExpressionParser parser = new ExpressionParser();
            Polynomial poly = new Polynomial(polyExpression, parser);
            Monomial first = new Monomial(firstMonomExpression, parser);
            Monomial second = new Monomial(secondMonomExpression, parser);
            Monomial third = new Monomial(thirdMonomExpression, parser);

            Monomial[] monomArray = poly.Monomials.ToArray();
            Assert.AreEqual(first.Expression,firstMonomExpression);
            Assert.AreEqual(second.Expression,secondMonomExpression);
            Assert.AreEqual(third.Expression,thirdMonomExpression);

        }
    }
}
