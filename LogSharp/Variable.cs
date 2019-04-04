using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace LogSharp
{
    public class Variable : IDisposable, IEnumerable
    {
        public static Variable _ = new Variable(0);

        private static int NextId = 1;
        private readonly int _id;

        internal IList values = new List<Object>();

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
            return obj is Variable && ((Variable)obj)._id == _id;
        }

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
