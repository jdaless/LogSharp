using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace LogSharp
{
    public class Fact : IFact
    {
        private readonly uint _parity;
        internal readonly object[] _args;
        internal readonly Rule _functor;
        public Fact(Rule r, object[] args)
        {
            _args = args;
            _parity = (uint)args.Length;
            _functor = r;
        }
        public Fact()
        {
            _args = Array.Empty<object>();
            _parity = 0;
            _functor = null;
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            //
            // See the full list of guidelines at
            //   http://go.microsoft.com/fwlink/?LinkID=85237
            // and also the guidance for operator== at
            //   http://go.microsoft.com/fwlink/?LinkId=85238
            //
            
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            
            var f = (Fact)obj;

            if (f._functor == null && this._functor == null)
            {
                return f == this;
            }
            if (f._functor != this._functor || f._parity != this._parity)
            {
                return false;
            }
            for(int i=0; i<_parity; i++)
            {
                if (!_args[i].Equals(f._args[i])) { return false; }
            }
            return true;
        }
        
        // override object.GetHashCode
        public override int GetHashCode()
        {
            // TODO: write your implementation of GetHashCode() here
            throw new System.NotImplementedException();
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

        MatchResult IFact.Match(IFact goal, World w)
        {
            // foreach(var arg in _args)
            // {
            //     if(arg is Variable)
            //     {

            //     }
            //     else
            //     {

            //     }
            // }
            if(!(goal is Fact)) return MatchResult.Inconclusive;
            var f = (Fact)goal;
            if(!(f.IsCompatable(this))) return MatchResult.Inconclusive;
            if(f._parity == 0)
            {
                return (f._functor == this._functor)?MatchResult.Satisfied:MatchResult.Inconclusive;
            }
            else if(f._parity == 1)
            {
                if(f._args[0] is Variable)
                {
                    if(this._args[0] is Variable) return MatchResult.Inconclusive;

                    ((Variable)f._args[0]).values.Add(this._args[0]);
                    return MatchResult.Satisfied;
                }
                else
                {
                    return f.Equals(this)?MatchResult.Satisfied: MatchResult.Inconclusive;
                }
            }
            return MatchResult.Inconclusive;
        }

        private bool IsCompatable(Fact f)
        {
            return this._parity == f._parity && this._functor == f._functor;
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
