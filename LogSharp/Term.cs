namespace LogSharp
{
    public abstract class Term
    {
        internal abstract MatchResult Match(Term goal, World w);
        internal abstract bool VariablesSatisfied();

        public Term Not()
        {
            return new Negation(this);
        }

        public Term Implies(Term t2)
        {
            return new Implication(this, t2);
        }

        public Term If(Term t2)
        {
            return t2.Implies(this);
        }

        public Term Conjoin(Term t2)
        {
            return new Conjunction(this, t2);
        }

        public Term Disjoin(Term t2)
        {
            return new Disjunction(this, t2);
        }

        public Term IFF(Term t2)
        {
            return this.Conjoin(t2).Disjoin(this.Not().Conjoin(t2.Not()));
        }

        public static Term operator >(Term r1, Term r2)
        {
            return r1.Implies(r2);
            //return (!r1) | r2;
        }

        public static Term operator <(Term r1, Term r2)
        {
            return r2.Implies(r1);
        }

        public static Term operator &(Term r1, Term r2)
        {
            return r1.Conjoin(r2);
        }

        public static Term operator ^(Term r1, Term r2)
        {
            return r1.Conjoin(r2);
        }

        public static Term operator |(Term r1, Term r2)
        {
            return r1.Disjoin(r2);
        }

        public static Term operator !(Term r1)
        {
            return r1.Not();
        }

        public static Term operator ~(Term r1)
        {
            return r1.Not();
        }


    }
}