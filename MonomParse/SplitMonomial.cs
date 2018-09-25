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
            StringBuilder str = new StringBuilder();
            for (int x = 0; x < expression.Length; x++)
            {
                if ((expression[x] == '-' || expression[x] == '+') && str.Length > 0)
                {
                        yield return str.ToString();
                        str.Clear();
                        if (expression[x] == '-')
                            str.Append(expression[x]);
                }
                else
                {
                    str.Append(expression[x]);
                }

            }
            if (str.Length>0)
                yield return str.ToString();

        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
