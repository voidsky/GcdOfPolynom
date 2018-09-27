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

        [TestCase(null, "0")]
        [TestCase("1", "1")]
        [TestCase("x^2", "x^2+0x+0")]
        [TestCase("-1x^2", "-1x^2+0x+0")]
        [TestCase("-x^5+x^2", "-x^5+0x^4+0x^3+x^2+0x+0")]
        [TestCase("-x^6-x^2-3", "-x^6+0x^5+0x^4+0x^3-x^2+0x-3")]
        [TestCase("5-2x^2+3x^3", "3x^3-2x^2+0x+5")]
        public void TestAddMissing(string polyExpression, string expectedResult)
        {
            ExpressionParser parser = new ExpressionParser();
            Polynomial poly = new Polynomial(polyExpression, parser);
            poly.SortAndFillWithMissing();
            Assert.AreEqual(expectedResult, poly.PolynomialString());
        }

        [TestCase(null, null)]
        [TestCase("x+1", 1)]
        [TestCase("x^8+x^100+1", 100)]
        [TestCase("x^3+x^2-x^1+1", 3)]
        [TestCase("5-2x^2+3x^3", 3)]
        public void TesPolyDegreeDegree(string polyExpression, int? expectedResult)
        {
            ExpressionParser parser = new ExpressionParser();
            Polynomial poly = new Polynomial(polyExpression, parser);
            Assert.AreEqual(expectedResult, poly.Degree());
        }

        [TestCase(null, null)]
        [TestCase("x+1", "x")]
        [TestCase("x^8+x^100+1", "x^100")]
        [TestCase("x^3+x^2-x^1+1", "x^3")]
        [TestCase("x^8-x^100+1", "-x^100")]
        [TestCase("5-2x^2+3x^3", "3x^3")]
        public void TesPolyHighestDegree(string polyExpression, string expectedResult)
        {
            ExpressionParser parser = new ExpressionParser();
            Polynomial poly = new Polynomial(polyExpression, parser);
            Assert.AreEqual(expectedResult, poly.GetWithHighestDegree()?.Expression);
        }


        [TestCase("x^8+x^100+1", "2x^8+5x^100+1", "-4x^100-1x^8")]
        [TestCase("10x^8-2x^100-5", "5x^8+5x^100+1", "-7x^100+5x^8-6")]
        [TestCase("10x^8", "5x^8", "5x^8")]
        [TestCase("-10x^8", "-5x^8", "-5x^8")]
        [TestCase("10x^8+5x^4", "2x^9+2x^4+5", "-2x^9+10x^8+3x^4-5")]

        public void TesPolySubtract(string firstPoly, string secondPoly, string expectedResult)
        {
            ExpressionParser parser = new ExpressionParser();
            Polynomial first = new Polynomial(firstPoly, parser);
            Polynomial second = new Polynomial(secondPoly, parser);
            first = first.Subtract(second);
            Assert.AreEqual(expectedResult, first.PolynomialString());
        }


        [TestCase("x^2+0x-1", "1", "x^2+0x-1")]
        [TestCase("x^2+0x-1", "0", "0x^2+0x+0")]
        [TestCase("x^2+0x-1", "-2", "-2x^2+0x+2")]
        public void TestPolyMultiply(string whatExpr, string byExptr, string resultExpr)
        {
            ExpressionParser parser = new ExpressionParser();
            Polynomial first = new Polynomial(whatExpr, parser);
            Monomial second = new Monomial(byExptr, parser);
            Polynomial result = first.MultiplyBy(second);
            Assert.AreEqual(resultExpr, result.PolynomialString());

        }


        [TestCase("6-2x^2+3x^3","x^2-1","3x-2", "3x+4")]
        [TestCase("2x+x^3-4x^2-3", "x+2", "x^2-6x+14", "-31")]
        [TestCase("4x^3-13x^2+2x-7", "x^2+3x-2", "4x-25", "85x-57")]
        [TestCase("2x^3+2x+7x^2+9", "2x+3", "x^2+2x-2", "15")]
        [TestCase("x^3+3x^2-4x-12", "x^2+x-6", "x+2", "")]
        [TestCase("x^3-4x^2+2x+5", "x-2", "x^2-2x-2", "1")]
        [TestCase("2x^3+4x^2-5", "x+3", "2x^2-2x+6", "-23")]
        [TestCase("2x^3-4x+7x^2+7", "x^2+2x-1", "2x+3", "-8x+10")]
        [TestCase("4x^3-2x^2-3", "2x^2-1", "2x-1", "2x-4")]
        [TestCase("x^2", "x", "x", "")]
        [TestCase("x^2+1", "x", "x", "1")]
        [TestCase("3x^3+4x+11", "x^2-3x+2", "3x+9", "25x-7")]
        public void TestPolyLongDivision(string whatExpr, string byExptr, string resultExpr, string reminderExpr)
        {
            ExpressionParser parser = new ExpressionParser();
            Polynomial first = new Polynomial(whatExpr, parser);
            Polynomial second = new Polynomial(byExptr, parser);
            Polynomial reminder;
            Polynomial result = second.Divide(first, second, out reminder);
            Assert.AreEqual(resultExpr, result.PolynomialString());
            Assert.AreEqual(reminderExpr, reminder.PolynomialString());
        }

        [TestCase("x^2+7x+6","x^2-5x-6","x+1")]
        public void TestPolyGcd(string whatExpr, string withExpr, string resultExpr)
        {
            ExpressionParser parser = new ExpressionParser();
            Polynomial first = new Polynomial(whatExpr, parser);
            Polynomial second = new Polynomial(withExpr, parser);
            //Polynomial reminder = first.Gcd(second);
            //Assert.AreEqual(resultExpr, reminder.PolynomialString());
        }




    }
}
