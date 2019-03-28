using System;

namespace LogSharp
{
    public class Rule : IFact
    {
        public Fact this[params object[] args]
        {
            get { return new Fact(this, args); }
        }

        bool IFact.Evaluate(World w)
        {
            return false;
        }

        bool IFact.Coerce(World w)
        {
            return ((IFact)this).Evaluate(w) ? true : w.Add(this);
        }

        /// <summary>
        /// Right implication, the rule is satisfied if the right argument 
        /// follows from the left one.
        /// </summary>
        /// <param name="r1">Implicator</param>
        /// <param name="r2">Implicans</param>
        /// <returns>A new rule representing that r1 implies r2</returns>
        public static Rule operator >(Rule r1, IFact r2)
        {
            return new ImpliedRule(r1,r2);
        }
        /// <summary>
        /// Left implication, the rule is satisfied if the left argument 
        /// follows from the right one. Similar to the Prolog :- operator.
        /// </summary>
        /// <param name="r1">Implicator</param>
        /// <param name="r2">Implicans</param>
        /// <returns>A new rule representing that r1 implies r2</returns>
        public static Rule operator <(Rule r1, IFact r2)
        {
            return new ImpliedRule(r2,r1);
        }

        public static Rule DoubleImply(Rule r1, IFact r2)
        {
            return (r1 > r2) ^ (r1 < r2);
        }

        public static Rule operator &(Rule r1, IFact r2)
        {
            return new ConjoinedRule(r1,r2);
        }

        public static Rule operator ^(Rule r1, IFact r2)
        {
            return r1 & r2;
        }

        public static Rule operator |(Rule r1, IFact r2)
        {
            return new DisjoinedRule(r1,r2);
        }

        public static Rule operator !(Rule r1)
        {
            return new NegatedRule(r1);
        }

        public static Rule operator ~(Rule r1)
        {
            return !r1;
        }

        internal class ConjoinedRule : Rule, IFact
        {
            private IFact _left;
            private IFact _right;
            public ConjoinedRule(IFact a, IFact b)
            {
                _left = a;
                _right = b;
            }
            bool IFact.Evaluate(World w)
            {
                return _left.Evaluate(w) & _right.Evaluate(w);
            }
            bool IFact.Coerce(World w)
            {
                return _left.Coerce(w) & _right.Coerce(w);
            }
        }

        internal class DisjoinedRule : Rule, IFact
        {
            private IFact _left;
            private IFact _right;
            public DisjoinedRule(IFact a, IFact b)
            {
                _left = a;
                _right = b;
            }
            bool IFact.Evaluate(World w)
            {
                return _left.Evaluate(w) | _right.Evaluate(w);
            }
        }

        internal class ImpliedRule : Rule, IFact
        {
            private IFact _left;
            private IFact _right;
            public ImpliedRule(IFact a, IFact b)
            {
                _left = a;
                _right = b;
            }
            bool IFact.Evaluate(World w)
            {
                return (!_left.Evaluate(w)) | _right.Evaluate(w);
            }
            bool IFact.Coerce(World w)
            {
                if(_left.Evaluate(w))
                {
                    return _right.Coerce(w);
                }
                if(!_right.Evaluate(w))
                {
                    return ((IFact) new NegatedRule(_left)).Coerce(w);
                }
                return true;
            }
        }

        internal class NegatedRule : Rule, IFact
        {
            private IFact _left;
            public NegatedRule(IFact a)
            {
                _left = a;
            }
            bool IFact.Evaluate(World w)
            {
                return !_left.Evaluate(w);
            }
            bool IFact.Coerce(World w)
            {
                return !_left.Evaluate(w);
            }
        }
    }
}
