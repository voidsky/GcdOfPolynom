﻿using System;
using System.Reflection;
using NUnit.Framework;

namespace MonomParse
{
    [TestFixture]
    public class MonomeTest
    {
        [TestCase(null, "")]
        [TestCase("", "")]
        [TestCase("x", "x")]
        [TestCase(" XX", "XX")]
        [TestCase(" 2XX ", "XX")]
        [TestCase(" -2XX^5 ", "XX")]
        [TestCase(" -2X X^-5 Y Y ", "X")]
        [TestCase("-5x^2", "x")]
        public void ParseMonomVariableResultsIn(string expression, string expectedResult)
        {
            IExpressionParser parser = new ExpressionParser();
            Monome monom = new Monome(expression, parser);
            string result = monom.Variable;
            Assert.AreEqual(expectedResult, result);
        }

        [TestCase("5", 5)]
        [TestCase(" 55 ", 55)]
        [TestCase(" -55 ", -55)]
        [TestCase("+1", 1)]
        [TestCase("-5x^2", -5)]
        public void ParseMonomCoefficientResultsIn(string expression, int expectedResult)
        {
            IExpressionParser parser = new ExpressionParser();
            Monome monom = new Monome(expression, parser);
            int? result = monom.Coefficient;
            Assert.AreEqual(expectedResult, result);
        }

        [TestCase("-5x^2", 2)]
        [TestCase("-5x", 1)]
        [TestCase("x", 1)]
        [TestCase("", 1)]
        [TestCase("-5x^-2", -2)]
        public void ParseMonomExponentResultsIn(string expression, int expectedResult)
        {
            IExpressionParser parser = new ExpressionParser();
            Monome monom = new Monome(expression, parser);
            int? result = monom.Exponent;
            Assert.AreEqual(expectedResult, result);
        }

        [TestCase(5,"x",2,"5x^2")]
        [TestCase(-5, " yy ", -2, "-5yy^-2")]
        [TestCase(1, "x",1, "1x^1")]
        [TestCase(null, "", null, "")]
        [TestCase(-5, "", null, "-5")]
        [TestCase(null, "", -5, "")]
        [TestCase(null, "x", null, "x")]
        public void CreateMonomeResultsIn(int? coefficient, string variable, int? exponent, string expression)
        {
            IExpressionParser parser = new ExpressionParser();
            Monome monom = new Monome(coefficient, variable, exponent, parser);
            Assert.AreEqual(monom.Expression, expression);

        }

    }
}
