using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace LogSharp
{
    public class Fact : ITermInternal
    {
        private readonly uint _arity;
        internal readonly object[] _args;
        private readonly MatchResult[] _satisfied;
        internal readonly Rule _functor;
        private readonly Guid _id;
        public Fact(Rule r, object[] args)
        {
            _args = args;
            _arity = (uint)args.Length;
            _functor = r;
            _id = Guid.NewGuid();
            _satisfied = Enumerable.Repeat(MatchResult.Compatible, (int)_arity).ToArray();
        }
        public Fact()
        {
            _args = Array.Empty<object>();
            _arity = 0;
            _functor = null;
            _id = Guid.NewGuid();
            _satisfied = new MatchResult[0];
        }

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
            for (int i = 0; i < _arity; i++)
            {
                if (!_args[i].Equals(f._args[i])) { return false; }
            }
            return true;
        }

        public override int GetHashCode()
        {
            // TODO: write your implementation of GetHashCode() here
            throw new System.NotImplementedException();
        }

        MatchResult ITermInternal.Match(ITermInternal goal, World w)
        {
            // let the rule handle the matching if the goal is one
            if (!(goal is Fact)) return goal.Match(this, w);

            var target = (Fact)goal;

            // facts that have nothing to do with eachother are
            // always logically compatible
            if (!(target.IsComparable(this))) return MatchResult.Compatible;

            // atoms
            if (target._arity == 0)
            {
                // atoms with the same id are a match, the goal is
                // satisfied. atoms without the same id are not a
                // match, but are logicallty compatible with eachother
                return (target._id == this._id) ?
                    MatchResult.Satisfied :
                    MatchResult.Compatible;
            }

            // predicates
            for (var i = 0; i < _arity; i++)
            {
                var targetVar = target._args[i] is Variable;
                var factVar = _args[i] is Variable;
                if (targetVar && factVar)
                {
                    // if both predicates have a variable in the same
                    // place then they're compatible.
                    _satisfied[i] = MatchResult.Compatible;
                }
                else if(targetVar && !factVar)
                {
                    // if the fact's argument is not a variable, then 
                    // its value is a satisfying variable for the goal.
                    ((Variable)target._args[i]).values.Add(_args[i]);
                    _satisfied[i] = MatchResult.Satisfied;
                }
                else if(!targetVar && factVar)
                {
                    // if the target's argument is not a variable, then
                    // its value is added to my argument
                    ((Variable)_args[i]).values.Add(target._args[i]);
                    _satisfied[i] = MatchResult.Satisfied;
                }
                else
                {
                    // neither argument is a variable, either satisfied
                    // if they're equal or incompatible if not
                    _satisfied[i] = target._args[i].Equals(_args[i]) ?
                        MatchResult.Satisfied :
                        MatchResult.Compatible;
                }
            }
            
            // if all the args are satisfied then the goal is satisfied
            // otherwise the goal is compatable
            return ((ITermInternal)this).VariablesSatisfied()?
                MatchResult.Satisfied:
                MatchResult.Compatible;
        }

        bool ITermInternal.VariablesSatisfied()
        {
            return _satisfied.All((mr) => mr == MatchResult.Satisfied);
        }

        private bool IsComparable(Fact f)        
        {
            return this._arity == f._arity && this._functor == f._functor;
        }

        public Rule Not()
        {
            return new Rule.NegatedRule(this);
        }

        public Rule Implies(ITerm t2)
        {
            return new Rule.ImpliedRule(this, (ITermInternal)t2);
        }

        public Rule If(ITerm t2)
        {
            return t2.Implies(this);
        }

        public Rule Conjoin(ITerm t2)
        {
            return new Rule.ConjoinedRule(this, (ITermInternal) t2);
        }

        public Rule Disjoin(ITerm t2)
        {
            return new Rule.DisjoinedRule(this, (ITermInternal) t2);
        }

        public Rule IFF(ITerm t2)
        {            
            return this.Conjoin(t2).Disjoin(this.Not().Conjoin(t2.Not()));
        }

        public static Rule operator >(Fact r1, ITerm r2)
        {
            return r1.Implies(r2);
        }

        public static Rule operator <(Fact r1, ITerm r2)
        {
            return r2.Implies(r1);
        }


        public static Rule operator &(Fact r1, ITerm r2)
        {
            return r1.Conjoin(r2);
        }

        public static Rule operator ^(Fact r1, ITerm r2)
        {
            return r1.Conjoin(r2);
        }

        public static Rule operator |(Fact r1, ITerm r2)
        {
            return r1.Disjoin(r2);
        }

        public static Rule operator !(Fact r1)
        {
            return r1.Not();
        }

        public static Rule operator ~(Fact r1)
        {
            return r1.Not();
        }
    }
}
