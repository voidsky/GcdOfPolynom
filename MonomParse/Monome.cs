﻿using System;
using System.Text.RegularExpressions;

namespace MonomParse
{
    internal class Monome
    {
        private string expression;
        private string variable;
        private int? coefficient;
        private int? exponent;
        private IExpressionParser parser;

        public Monome(string expression, IExpressionParser parser)
        {
            this.parser = parser;
            this.ParseExpression(expression);
        }

        public Monome(int? coefficient, string variable, int? exponent, IExpressionParser parser)
        {
            this.parser = parser;
            this.coefficient = coefficient;
            this.variable = variable?.Trim();
            this.exponent = exponent;
            this.expression = parser.CombineStringExpression(this.coefficient, this.variable, this.exponent);
        }

        private void ParseExpression(string fromExpression)
        {
            this.expression = fromExpression;
            this.variable = parser.ExtractVariable(fromExpression);
            this.coefficient = parser.ExtractCoefficient(fromExpression);
            this.exponent = parser.ExtractExponent(fromExpression);
        }

        public string Variable => this.variable;
        public int? Coefficient => this.coefficient;
        public int? Exponent => this.exponent;
        public string Expression => this.expression;

    }
}