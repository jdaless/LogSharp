using System;
using System.Collections.Generic;
using System.Text;

namespace LogSharp
{
    public class Fact : IFact
    {
        private readonly uint _parity;
        internal readonly object[] _args;
        private readonly Rule _rule;
        public Fact(Rule r, object[] args)
        {
            _args = args;
            _parity = (uint)args.Length;
            _rule = r;
        }
        public Fact()
        {
            _args = Array.Empty<object>();
            _parity = 0;
            _rule = null;
        }

        public bool TestEquality(Fact f)
        {
            if (f._rule == null && this._rule == null)
            {
                return f == this;
            }
            if (f._rule != this._rule || f._parity != this._parity)
            {
                return false;
            }
            for(int i=0; i<_parity; i++)
            {
                if (!_args[i].Equals(f._args[i])) { return false; }
            }
            return true;
        }

        bool IFact.Evaluate(World w)
        {
            return w.ContainsFact(this);
        }

        bool IFact.Coerce(World w)
        {
            if(!w.ContainsFact(this))
            {
                return w.Add(this);
            }
            return true;
        }

        /// <summary>
        /// Right implication, the rule is satisfied if the right argument 
        /// follows from the left one.
        /// </summary>
        /// <param name="r1">Implicator</param>
        /// <param name="r2">Implicans</param>
        /// <returns>A new rule representing that r1 implies r2</returns>
        public static Rule operator >(Fact r1, IFact r2)
        {
            return new Rule.ImpliedRule(r1,r2);
        }
        /// <summary>
        /// Left implication, the rule is satisfied if the left argument 
        /// follows from the right one.
        /// </summary>
        /// <param name="r1">Implicator</param>
        /// <param name="r2">Implicans</param>
        /// <returns>A new rule representing that r1 implies r2</returns>
        public static Rule operator <(Fact r1, IFact r2)
        {
            return new Rule.ImpliedRule(r2,r1);
        }

        public static Rule DoubleImply(Fact r1, IFact r2)
        {
            return (r1 > r2) ^ (r1 < r2);
        }

        public static Rule operator &(Fact r1, IFact r2)
        {
            return new Rule.ConjoinedRule(r1, r2);
        }

        public static Rule operator ^(Fact r1, IFact r2)
        {
            return r1 & r2;
        }

        public static Rule operator |(Fact r1, IFact r2)
        {
            return new Rule.DisjoinedRule(r1,r2);
        }

        public static Rule operator !(Fact r1)
        {
            return new Rule.NegatedRule(r1);
        }

        public static Rule operator ~(Fact r1)
        {
            return !r1;
        }
    }
}
