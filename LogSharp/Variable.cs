using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace LogSharp
{
    public class Variable : IDisposable, IEnumerable<Object>
    {
        public static Variable _ = new Variable(0);

        private static int NextId = 1;
        private readonly int _id;

        internal IList<Object> values = new List<Object>();

        public Variable()
        {
            _id = NextId;
            NextId++;
        }

        private Variable(int id)
        {
            _id = id;
        }

        public override bool Equals(Object obj)
        {
            return obj is Variable && obj.GetHashCode() == this.GetHashCode();
        }

        public override int GetHashCode()
        {
            return _id;
        }

        public void Dispose()
        {
        }

        public IEnumerator<Object> GetEnumerator()
        {
            return values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return values.GetEnumerator();
        }
    }
}
