﻿using System;

namespace LogSharp
{
    public class Rule : IFactInternal
    {
        public static Rule Equality = new Rule();

        public Fact this[params object[] args]
        {
            get
            {
                return new Fact(this, args);
            }
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
            return new ImpliedRule(r1, (IFactInternal)r2);
            //return (!r1) | r2;
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
            return new ImpliedRule((IFactInternal)r2, r1);
        }

        public static Rule IFF(Rule r1, IFact r2)
        {
            return (r1 > r2) & (r1 < r2);
        }

        MatchResult IFactInternal.Match(IFactInternal goal, World w)
        {
            throw new NotImplementedException();
        }

        bool IFactInternal.VariablesSatisfied()
        {
            throw new NotImplementedException();
        }

        public static Rule operator &(Rule r1, IFact r2)
        {
            return new ConjoinedRule(r1, (IFactInternal)r2);
        }

        public static Rule operator ^(Rule r1, IFact r2)
        {
            return r1 & r2;
        }

        public static Rule operator |(Rule r1, IFact r2)
        {
            return new DisjoinedRule(r1, (IFactInternal)r2);
        }

        public static Rule operator !(Rule r1)
        {
            return new NegatedRule(r1);
        }

        public static Rule operator ~(Rule r1)
        {
            return !r1;
        }

        internal class ConjoinedRule : Rule, IFact, IFactInternal
        {
            private IFactInternal _left;
            private IFactInternal _right;
            public ConjoinedRule(IFactInternal a, IFactInternal b)
            {
                _left = a;
                _right = b;
            }

            MatchResult IFactInternal.Match(IFactInternal goal, World w)
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

            bool IFactInternal.VariablesSatisfied()
            {
                return _left.VariablesSatisfied() && _right.VariablesSatisfied();
            }
        }

        internal class DisjoinedRule : Rule, IFactInternal
        {
            private IFactInternal _left;
            private IFactInternal _right;
            public DisjoinedRule(IFactInternal a, IFactInternal b)
            {
                _left = a;
                _right = b;
            }

            MatchResult IFactInternal.Match(IFactInternal goal, World w)
            {
                var con = MatchResult.Incompatible;
                var l = _left.Match(goal, w);
                var r = _right.Match(goal, w);
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
                return con;
            }

            bool IFactInternal.VariablesSatisfied()
            {
                return _left.VariablesSatisfied() && _right.VariablesSatisfied();
            }
        }

        internal class ImpliedRule : Rule, IFactInternal
        {
            private IFactInternal _left;
            private IFactInternal _right;
            public ImpliedRule(IFactInternal a, IFactInternal b)
            {
                _left = a;
                _right = b;
            }

            MatchResult IFactInternal.Match(IFactInternal goal, World w)
            {
                var l = _left.Match(goal, w);
                var r = _right.Match(goal, w);
                Console.WriteLine(l + " => " + r);
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

            bool IFactInternal.VariablesSatisfied()
            {
                return _left.VariablesSatisfied() && _right.VariablesSatisfied();
            }
        }

        internal class NegatedRule : Rule, IFactInternal
        {
            private IFactInternal _left;
            public NegatedRule(IFactInternal a)
            {
                _left = a;
            }

            MatchResult IFactInternal.Match(IFactInternal goal, World w)
            {
                var match = _left.Match(goal, w);
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

            bool IFactInternal.VariablesSatisfied()
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
