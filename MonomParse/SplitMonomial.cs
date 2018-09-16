using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MonomialParse;

namespace MonomParse
{
    public class MonomialStrings : IEnumerable<String>
    {
        private string expression;

        public MonomialStrings(string expression)
        {
            this.expression = expression??"";
        }

        public IEnumerator<String> GetEnumerator()
        {
            char[] separators = {'+', '-'};
            int position = 0;
            int start = 0;
            do
            {
                position = expression.IndexOfAny(separators, start);
                if (position >= 0)
                {
                    yield return expression.Substring(start, position - start).Trim();
                    start = position + 1;
                } else if (position == -1 && (expression.Length - start >0))
                {
                    yield return expression.Substring(start, expression.Length - start).Trim();
                }

            } while (position > 0);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
