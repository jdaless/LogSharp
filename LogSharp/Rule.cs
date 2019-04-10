using System;

namespace LogSharp
{
    public class Rule : ITermInternal
    {
        public static Rule Equality = new Rule();

        public Fact this[params object[] args]
        {
            get
            {
                return new Fact(this, args);
            }
        }

        MatchResult ITermInternal.Match(ITermInternal goal, World w)
        {
            throw new NotImplementedException();
        }

        bool ITermInternal.VariablesSatisfied()
        {
            throw new NotImplementedException();
        }

        public Rule Not()
        {
            return new NegatedRule(this);
        }

        public Rule Implies(ITerm t2)
        {
            return new ImpliedRule(this, (ITermInternal)t2);
        }

        public Rule If(ITerm t2)
        {
            return t2.Implies(this);
        }

        public Rule Conjoin(ITerm t2)
        {
            return new ConjoinedRule(this, (ITermInternal)t2);
        }

        public Rule Disjoin(ITerm t2)
        {
            return new DisjoinedRule(this, (ITermInternal)t2);
        }

        public Rule IFF(ITerm t2)
        {
            return this.Conjoin(t2).Disjoin(this.Not().Conjoin(t2.Not()));
        }

        public static Rule operator >(Rule r1, ITerm r2)
        {
            return r1.Implies(r2);
            //return (!r1) | r2;
        }
        
        public static Rule operator <(Rule r1, ITerm r2)
        {
            return r2.Implies(r1);
        }
        
        public static Rule operator &(Rule r1, ITerm r2)
        {
            return r1.Conjoin(r2);
        }

        public static Rule operator ^(Rule r1, ITerm r2)
        {
            return r1.Conjoin(r2);
        }

        public static Rule operator |(Rule r1, ITerm r2)
        {
            return r1.Disjoin(r2);
        }

        public static Rule operator !(Rule r1)
        {
            return r1.Not();
        }

        public static Rule operator ~(Rule r1)
        {
            return r1.Not();
        }

        internal class ConjoinedRule : Rule, ITermInternal
        {
            private ITermInternal _left;
            private ITermInternal _right;
            public ConjoinedRule(ITermInternal a, ITermInternal b)
            {
                _left = a;
                _right = b;
            }

            MatchResult ITermInternal.Match(ITermInternal goal, World w)
            {
                var sat = MatchResult.Incompatible;
                var l = _left.Match(goal, w);
                var r = _right.Match(goal, w);
                switch (l)
                {
                    case MatchResult.Contradicted:
                        return l;
                    case MatchResult.Satisfied:
                        sat = l;
                        break;
                }
                switch (r)
                {
                    case MatchResult.Contradicted:
                        return r;
                    case MatchResult.Satisfied:
                        if (sat == r) return r;
                        break;
                }
                return MatchResult.Incompatible;
            }

            bool ITermInternal.VariablesSatisfied()
            {
                return _left.VariablesSatisfied() && _right.VariablesSatisfied();
            }
        }

        internal class DisjoinedRule : Rule, ITermInternal
        {
            private ITermInternal _left;
            private ITermInternal _right;
            public DisjoinedRule(ITermInternal a, ITermInternal b)
            {
                _left = a;
                _right = b;
            }

            MatchResult ITermInternal.Match(ITermInternal goal, World w)
            {
                var con = MatchResult.Compatible;
                var l = _left.Match(goal, w);
                var r = _right.Match(goal, w);
                Console.WriteLine($"{l} v {r}");
                switch (l)
                {
                    case MatchResult.Satisfied:
                        return l;
                    case MatchResult.Contradicted:
                        con = l;
                        break;
                }
                switch (r)
                {
                    case MatchResult.Satisfied:
                        return r;
                    case MatchResult.Contradicted:
                        if (con == r) return r;
                        break;
                }
                return MatchResult.Compatible;
            }

            bool ITermInternal.VariablesSatisfied()
            {
                return _left.VariablesSatisfied() && _right.VariablesSatisfied();
            }
        }

        internal class ImpliedRule : Rule, ITermInternal
        {
            private ITermInternal _left;
            private ITermInternal _right;
            public ImpliedRule(ITermInternal a, ITermInternal b)
            {
                _left = a;
                _right = b;
            }

            MatchResult ITermInternal.Match(ITermInternal goal, World w)
            {
                var l = _left.Match(goal, w);
                var r = _right.Match(goal, w);
                //Console.WriteLine(l + " => " + r);
                if (l == MatchResult.Contradicted)
                    return MatchResult.Satisfied;
                else if (r == MatchResult.Satisfied)
                    return MatchResult.Satisfied;
                else if (l == MatchResult.Satisfied && r == MatchResult.Contradicted)
                    return MatchResult.Contradicted;
                else if (l == MatchResult.Satisfied && r == MatchResult.Incompatible)
                    return MatchResult.Incompatible;

                return MatchResult.Compatible;
            }

            bool ITermInternal.VariablesSatisfied()
            {
                return _left.VariablesSatisfied() && _right.VariablesSatisfied();
            }
        }

        internal class NegatedRule : Rule, ITermInternal
        {
            private ITermInternal _left;
            public NegatedRule(ITermInternal a)
            {
                _left = a;
            }

            MatchResult ITermInternal.Match(ITermInternal goal, World w)
            {
                var match = goal.Match(_left, w);
                MatchResult res;
                switch (match)
                {
                    case MatchResult.Satisfied:
                        res = MatchResult.Contradicted;
                        break;
                    case MatchResult.Contradicted:
                        res = MatchResult.Satisfied;
                        break;
                    case MatchResult.Incompatible:
                        res = MatchResult.Compatible;
                        break;
                    case MatchResult.Compatible:
                        res = MatchResult.Compatible;
                        break;
                    default:
                        res = 0;
                        break;
                }
                return res;
            }

            bool ITermInternal.VariablesSatisfied()
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
