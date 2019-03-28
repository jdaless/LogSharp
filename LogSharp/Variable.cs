using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace LogSharp
{
    public class Variable<T> : IDisposable, IEnumerable<T>
    {
        public static Variable<Object> _ = new Variable<Object>();

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
