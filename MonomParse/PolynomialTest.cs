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
    }
}
