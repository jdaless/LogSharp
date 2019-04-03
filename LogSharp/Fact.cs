using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace LogSharp
{
    public class Fact : IFact
    {
        private readonly uint _arity;
        internal readonly object[] _args;
        internal readonly Rule _functor;
        private readonly Guid _id;
        public Fact(Rule r, object[] args)
        {
            _args = args;
            _arity = (uint)args.Length;
            _functor = r;
            _id = Guid.NewGuid();
        }
        public Fact()
        {
            _args = Array.Empty<object>();
            _arity = 0;
            _functor = null;
            _id = Guid.NewGuid();
        }

        // override object.Equals
        public override bool Equals(object obj)
        {            
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            
            var f = (Fact)obj;

            if (f._functor == null && this._functor == null)
            {
                return f._id == this._id;
            }
            if (f._functor != this._functor || f._arity != this._arity)
            {
                return false;
            }
            for(int i=0; i<_arity; i++)
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

        MatchResult IFact.Match(IFact goal, World w)
        {
            if(!(goal is Fact)) return goal.Match(this, w);

            var f = (Fact)goal;
            if(!(f.IsCompatable(this))) return MatchResult.Inconclusive;
            if(f._arity == 0)
            {
                return (f._id == this._id)?MatchResult.Satisfied:MatchResult.Inconclusive;
            }
            else if(f._arity == 1)
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
            return MatchResult.Contradicted;
        }

        private bool IsCompatable(Fact f)
        {
            return this._arity == f._arity && this._functor == f._functor;
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
