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

        [TestCase("2x^2+x+1","2x^2","x","1")]
        [TestCase("-x^3-x-1","-x^3","-x","-1")]
        [TestCase("5-2x^2+3x^3", "5", "-2x^2", "3x^3")]
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

            var monomArray = poly.Monomials.ToArray();
            Assert.AreEqual(first.Expression, monomArray[0].Expression );
            Assert.AreEqual(second.Expression, monomArray[1].Expression );
            Assert.AreEqual(third.Expression, monomArray[2].Expression);
        }

        [TestCase("1","1")]
        [TestCase("x^2", "x^2")]
        [TestCase("x+x^2", "x^2+x")]
        [TestCase("5-2x^2+3x^3", "3x^3-2x^2+5")]
        [TestCase("-x^5+x^2", "-x^5+x^2")]
        public void TestSortPolynomial(string polyExpression, string polyExpressionSorted)
        {
            ExpressionParser parser = new ExpressionParser();
            Polynomial poly = new Polynomial(polyExpression, parser);
            poly.SortDescending();
            Assert.AreEqual(polyExpressionSorted, poly.PolynomialString());


        }

        [TestCase(null, "")]
        [TestCase("1", "1")]
        [TestCase("x^2", "x^2+0x")]
        [TestCase("-1x^2", "-1x^2+0x")]
        [TestCase("-x^5+x^2", "-x^5+0x^4+0x^3+x^2+0x")]
        [TestCase("-x^6-x^2-3", "-x^6+0x^5+0x^4+0x^3-x^2-3")]
        public void TestAddMissing(string polyExpression, string expectedresult)
        {
            ExpressionParser parser = new ExpressionParser();
            Polynomial poly = new Polynomial(polyExpression, parser);
            Polynomial polyWithMissing = poly.SortedAndFilledWithMissing();
            Assert.AreEqual(expectedresult, polyWithMissing.PolynomialString());
        }
    }
}
