using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonomialParse;
using NUnit.Framework;

namespace MonomParse
{
    [TestFixture]
    public class MonomialsOperationsTest
    {
        #region Test monomials addition
        [Test]
        public void MonomialsAddResultsIn3X2()
        {
            IExpressionParser parser = new ExpressionParser();
            Monomial monomial = new Monomial(1, "x", 2, parser);
            Monomial monomialToAdd = new Monomial(2, "x", 2, parser);
            Monomial result = monomial.AddMonomialWithSameVariable(monomialToAdd);
            Assert.AreEqual("3x^2", result.Expression);
        }

        [Test]
        public void MonomialsAddResultsIn3X()
        {
            IExpressionParser parser = new ExpressionParser();
            Monomial monomial = new Monomial(1, "x", null, parser);
            Monomial monomialToAdd = new Monomial(2, "x", null, parser);
            Monomial result = monomial.AddMonomialWithSameVariable(monomialToAdd);
            Assert.AreEqual("3x", result.Expression);
        }

        [Test]
        public void MonomialsAddResultsIn3()
        {
            IExpressionParser parser = new ExpressionParser();
            Monomial monomial = new Monomial(1, "", null, parser);
            Monomial monomialToAdd = new Monomial(2, "", null, parser);
            Monomial result = monomial.AddMonomialWithSameVariable(monomialToAdd);
            Assert.AreEqual("3", result.Expression);
        }

        [Test]
        public void MonomialsAddResultsInException()
        {
            IExpressionParser parser = new ExpressionParser();
            Monomial monomial = new Monomial(1, "x", 2, parser);
            Monomial monomialToAdd = new Monomial(2, "y", 2, parser);
            Assert.Throws<InvalidOperationWithMonomialsException>(() =>
                monomial.AddMonomialWithSameVariable(monomialToAdd));
        }

        [Test]
        public void MonomialsAddResultsInException2()
        {
            IExpressionParser parser = new ExpressionParser();
            Monomial monomial = new Monomial(1, "x", 2, parser);
            Monomial monomialToAdd = new Monomial(2, "x", 3, parser);
            Assert.Throws<InvalidOperationWithMonomialsException>(() =>
                monomial.AddMonomialWithSameVariable(monomialToAdd));
        }
        #endregion

        #region  Test monomials subtraction      
        [Test]
        public void MonomialsSubtractResultsIn_1()
        {
            IExpressionParser parser = new ExpressionParser();
            Monomial monomial = new Monomial(1, "x", 2, parser);
            Monomial monomialToSubtract = new Monomial(2, "x", 2, parser);
            Monomial result = monomial.SubtractMonomialWithSameVariable(monomialToSubtract);
            Assert.AreEqual("-1x^2", result.Expression);
        }

        [Test]
        public void MonomialsSubtractResultsIn_1X()
        {
            IExpressionParser parser = new ExpressionParser();
            Monomial monomial = new Monomial(1, "x", null, parser);
            Monomial monomialToSubtract = new Monomial(2, "x", null, parser);
            Monomial result = monomial.SubtractMonomialWithSameVariable(monomialToSubtract);
            Assert.AreEqual("-1x", result.Expression);
        }

        [Test]
        public void MonomialsSubtractResultsIn3()
        {
            IExpressionParser parser = new ExpressionParser();
            Monomial monomial = new Monomial(5, "", null, parser);
            Monomial monomialToSubtract = new Monomial(2, "", null, parser);
            Monomial result = monomial.SubtractMonomialWithSameVariable(monomialToSubtract);
            Assert.AreEqual("3", result.Expression);
        }

        [Test]
        public void MonomialsSubtractResultsInException()
        {
            IExpressionParser parser = new ExpressionParser();
            Monomial monomial = new Monomial(1, "x", 2, parser);
            Monomial monomialToSubtract = new Monomial(2, "y", 2, parser);
            Assert.Throws<InvalidOperationWithMonomialsException>(() =>
                monomial.SubtractMonomialWithSameVariable(monomialToSubtract));
        }

        [Test]
        public void MonomialsSubtractResultsInException2()
        {
            IExpressionParser parser = new ExpressionParser();
            Monomial monomial = new Monomial(1, "x", 2, parser);
            Monomial monomialToSubtract = new Monomial(2, "x", 3, parser);
            Assert.Throws<InvalidOperationWithMonomialsException>(() =>
                monomial.SubtractMonomialWithSameVariable(monomialToSubtract));
        }
        #endregion

        #region Test monomials division
        [Test]
        [TestCase(1,"x",2,  2,"x",2, "0.5")]
        [TestCase(null,"x",5,  null,"x",3, "x^2")]
        [TestCase(null, "x", null, null, "x", null, "1")]
        [TestCase(5, null, null, 2, null, null, "2.5")]
        [TestCase(5, "x", 1, 2, "x", 2, "2.5x^-1")]
        [TestCase(5, "x", 2, 2, "x", 2, "2.5")]
        [TestCase(-5, "x", -2, 2, "x", 2, "-2.5x^-4")]
        public void MonomialsDivisionTest1(decimal? coefficient, string variable, int? exponent,
            int? coefficient2, string variable2, int? exponent2,
            string expressionResult)
        {
            IExpressionParser parser = new ExpressionParser();
            Monomial monomial = new Monomial(coefficient, variable, exponent, parser);
            Monomial divisor = new Monomial(coefficient2, variable2, exponent2, parser);
            Monomial result = monomial.DivideMonomialWithSameVariable(divisor);
            Assert.AreEqual(expressionResult, result.Expression);
        }

        [Test]
        public void MonomialsDivisionResultsInException2()
        {
            IExpressionParser parser = new ExpressionParser();
            Monomial monomial = new Monomial(1, "x", 2, parser);
            Monomial divisor = new Monomial(2, "y", 3, parser);
            Assert.Throws<InvalidOperationWithMonomialsException>(() =>
                monomial.DivideMonomialWithSameVariable(divisor));
        }

        [Test]
        public void MonomialsDivisionTest2()
        {
            IExpressionParser parser = new ExpressionParser();
            Monomial monomial = new Monomial(5, "x", 8, parser);
            Monomial divisor = new Monomial(0, "x", 2, parser);
            Assert.Throws<DivideByZeroException>(() =>
                monomial.DivideMonomialWithSameVariable(divisor));
        }

        [Test]
        public void MonomialsDivisionTest3()
        {
            IExpressionParser parser = new ExpressionParser();
            Monomial monomial = new Monomial(0, "x", 1, parser);
            Monomial divisor = new Monomial(2, "x", 2, parser);
            Monomial result = monomial.DivideMonomialWithSameVariable(divisor);
            Assert.AreEqual("0", result.Expression);
        }

        [TestCase("x^2","x^2","x^4")]
        [TestCase("2x^2", "3x^3", "6x^5")]
        [TestCase("-2x^2", "3x^3", "-6x^5")]
        [TestCase("0x^2", "3x^3", "0x^5")]
        public void TestMonomialMultiplyBy(string firstExpression, string secondExpression, string resultExpression)
        {
            IExpressionParser parser = new ExpressionParser();
            Monomial firstMonomial = new Monomial(firstExpression, parser);
            Monomial secondMonomial = new Monomial(secondExpression, parser);
            Monomial result = firstMonomial.MultiplyBy(secondMonomial);
            Assert.AreEqual(resultExpression, result.Expression);

        }
        #endregion
    }
}
