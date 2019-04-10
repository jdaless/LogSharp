using System;

namespace LogSharp
{
    public class Predicate
    {
        public static Predicate Equality = new Predicate();

        public Fact this[params object[] args]
        {
            get
            {
                return new Fact(this, args);
            }
        }
    }

    internal class Conjunction : Term
    {
        private Term _left;
        private Term _right;
        public Conjunction(Term a, Term b)
        {
            _left = a;
            _right = b;
        }

        internal override MatchResult Match(Term goal, World w)
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

        internal override bool VariablesSatisfied()
        {
            return _left.VariablesSatisfied() && _right.VariablesSatisfied();
        }
    }

    internal class Disjunction : Term
    {
        private Term _left;
        private Term _right;
        public Disjunction(Term a, Term b)
        {
            _left = a;
            _right = b;
        }

        internal override MatchResult Match(Term goal, World w)
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

        internal override bool VariablesSatisfied()
        {
            return _left.VariablesSatisfied() && _right.VariablesSatisfied();
        }
    }

    internal class Implication : Term
    {
        private Term _left;
        private Term _right;
        public Implication(Term a, Term b)
        {
            _left = a;
            _right = b;
        }

        internal override MatchResult Match(Term goal, World w)
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

        internal override bool VariablesSatisfied()
        {
            return _left.VariablesSatisfied() && _right.VariablesSatisfied();
        }
    }

    internal class Negation : Term
    {
        private Term _left;
        public Negation(Term a)
        {
            _left = a;
        }

        internal override MatchResult Match(Term goal, World w)
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

        internal override bool VariablesSatisfied()
        {
            return _left.VariablesSatisfied();
        }
    }
}
