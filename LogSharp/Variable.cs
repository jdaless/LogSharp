using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace LogSharp
{
    public class Variable : IDisposable, IEnumerable
    {
        public static Variable _ = new Variable();

        internal IList values = new List<Object>();

        public void Dispose()
        {
        }

        public IEnumerator GetEnumerator()
        {
            return values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return values.GetEnumerator();
        }
    }
}
