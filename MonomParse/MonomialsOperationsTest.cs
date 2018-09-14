﻿using System;
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
        public void MonomialsDivisionTest1()
        {
            IExpressionParser parser = new ExpressionParser();
            Monomial monomial = new Monomial(1, "x", 2, parser);
            Monomial divisor = new Monomial(2, "x", 2, parser);
            Monomial result = monomial.DivideMonomialWithSameVariable(divisor);
            Assert.AreEqual("0.5", result.Expression);
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
        #endregion
    }
}