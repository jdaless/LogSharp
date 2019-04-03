using System;

namespace LogSharp
{
    public class Rule : IFact
    {
        public static Rule Equality;

        public Fact this[params object[] args]
        {
            get { return new Fact(this, args); }
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

        public bool Evaluate(World w)
        {
            throw new NotImplementedException();
        }

        public bool Coerce(World w)
        {
            throw new NotImplementedException();
        }

        public MatchResult Match(IFact goal, World w)
        {
            throw new NotImplementedException();
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

            public new MatchResult Match(IFact goal, World w)
            {
                var sat = MatchResult.Inconclusive;
                var l = _left.Match(goal, w);
                var r = _right.Match(goal, w);
                switch(l)
                {
                    case MatchResult.Contradicted:
                        return l;
                    case MatchResult.Satisfied:
                        sat = l;
                        break;
                }
                switch(r)
                {
                    case MatchResult.Contradicted:
                        return r;
                    case MatchResult.Satisfied:
                        if(sat == r) return r;
                        break;
                }
                return sat;
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

            public new MatchResult Match(IFact goal, World w)
            {
                var con = MatchResult.Inconclusive;
                var l = _left.Match(goal, w);
                var r = _right.Match(goal, w);
                switch(l)
                {
                    case MatchResult.Satisfied:
                        return l;
                    case MatchResult.Contradicted:
                        con = l;
                        break;
                }
                switch(r)
                {
                    case MatchResult.Satisfied:
                        return r;
                    case MatchResult.Contradicted:
                        if(con == r) return r;
                        break;
                }
                return con;
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

            public new MatchResult Match(IFact goal, World w)
            {
                var imp = MatchResult.Inconclusive;
                var l = _left.Match(goal, w);
                var r = _right.Match(goal, w);
                switch(l)
                {
                    // a false implicator means the implication is always true
                    case MatchResult.Contradicted:
                        return MatchResult.Satisfied;
                    case MatchResult.Satisfied:
                        imp = l;
                        break;
                }
                switch(r)
                {
                    // a true implicans means the implication is always true
                    case MatchResult.Satisfied:
                        return r;
                    case MatchResult.Contradicted:
                        if(imp == MatchResult.Satisfied) return r;
                        break;
                }
                return imp;
            }
        }

        internal class NegatedRule : Rule, IFact
        {
            private IFact _left;
            public NegatedRule(IFact a)
            {
                _left = a;
            }

            public new MatchResult Match(IFact goal, World w)
            {
                return _left.Match(goal, w)==MatchResult.Satisfied?
                            MatchResult.Contradicted:
                            MatchResult.Satisfied;
            }
        }
    }
}
