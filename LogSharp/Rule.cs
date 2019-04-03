using System;

namespace LogSharp
{
    public class Rule : IFact
    {
        public static Rule Equality = new Rule();

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

        MatchResult IFact.Match(IFact goal, World w)
        {
            throw new NotImplementedException();
        }

        bool IFact.VariablesSatisfied()
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

            MatchResult IFact.Match(IFact goal, World w)
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
                return MatchResult.Inconclusive;
            }
            
            bool IFact.VariablesSatisfied()
            {
                return _left.VariablesSatisfied() && _right.VariablesSatisfied();
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

            MatchResult IFact.Match(IFact goal, World w)
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
            
            bool IFact.VariablesSatisfied()
            {
                return _left.VariablesSatisfied() && _right.VariablesSatisfied();
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

            MatchResult IFact.Match(IFact goal, World w)
            {
                var imp = MatchResult.Inconclusive;
                var l = _left.Match(goal, w);
                var r = _right.Match(goal, w);
                switch(l)
                {
                    // a false implicator means the implication is always true
                    case MatchResult.Contradicted:
                        return MatchResult.Satisfied;
                    case MatchResult.Inconclusive:
                        return MatchResult.Satisfied;
                }
                switch(r)
                {
                    // a true implicans means the implication is always true
                    case MatchResult.Satisfied:
                        return r;
                    case MatchResult.Contradicted:
                        if(l == MatchResult.Satisfied) return r;
                        break;
                }
                return MatchResult.Inconclusive;
            }

            bool IFact.VariablesSatisfied()
            {
                return _left.VariablesSatisfied() && _right.VariablesSatisfied();
            }
        }

        internal class NegatedRule : Rule, IFact
        {
            private IFact _left;
            public NegatedRule(IFact a)
            {
                _left = a;
            }

            MatchResult IFact.Match(IFact goal, World w)
            {
                return _left.Match(goal, w)==MatchResult.Satisfied?
                            MatchResult.Contradicted:
                            MatchResult.Satisfied;
            }

            bool IFact.VariablesSatisfied()
            {
                return _left.VariablesSatisfied();
            }
        }

        // internal class CustomRule : Rule, IFact
        // {
        //     private Func<IFact, World, MatchResult> _match;
        //     public new Fact this[params object[] args]
        //     {
        //         get
        //         {
        //             return 
        //         }
        //     }
        //     public CustomRule(Func<IFact, World, MatchResult> match)
        //     {
        //         _match = match;
        //     }

        //     MatchResult IFact.Match(IFact goal, World w)
        //     {
        //         return _match.Invoke(goal, w);
        //     }
        // }
    }
}
