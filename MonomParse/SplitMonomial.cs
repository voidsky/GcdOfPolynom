using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MonomParse
{
    public class MonomialStrings : IEnumerable<string>
    {
        private readonly string expression;

        public MonomialStrings(string expression)
        {
            this.expression = expression ?? "";
        }

        public IEnumerator<string> GetEnumerator()
        {
            var str = new StringBuilder();
            foreach (var character in expression)
                if ((character == '-' || character == '+') && str.Length > 0)
                {
                    yield return str.ToString();
                    str.Clear();
                    if (character == '-')
                        str.Append(character);
                }
                else
                {
                    str.Append(character);
                }

            if (str.Length > 0)
                yield return str.ToString();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}