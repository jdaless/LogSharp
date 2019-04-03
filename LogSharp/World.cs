using System;
using System.Collections.Generic;
using System.Linq;

namespace LogSharp
{
    public class World
    {
        private readonly IList<IFact> _state = new List<IFact>();


        public World()
        {
            this.Add(new Rule()["red"]);
            // using(var x = new Variable())
            // {
            //     this.Add(Rule.Equality[x, x]);
            // }
        }

        /// <summary>
        /// Adds a new rule to the state of the world. Returns true on success,
        /// returns false and fails to add the rule if the new rule would 
        /// create a contradiction.
        /// </summary>
        public bool Add(IFact r)
        {
            if (this.NonContradict(r) == MatchResult.Contradicted)
                return false;
            _state.Add(r);
            return true;
        }

        /// <summary>
        /// Evaluates whether or not a rule creates a contradiction in the world
        /// without adding it to the state of the world. Will set values of variables
        /// in the rules with all possible values.
        /// </summary>
        public bool Query(IFact goal)
        {
            return this.NonContradict(goal).HasFlag(MatchResult.Satisfied);
        }

        private MatchResult NonContradict(IFact goal)
        {
            var sat = false;
            Console.WriteLine("\n_state: " + _state.Count());
            MatchResult res = MatchResult.WeaklyContradicted;
            foreach (var f in _state)
            {
                Console.WriteLine(goal.GetType().Name
                    + " to "
                    + f.GetType().Name
                    + ": ");
                var result = f.Match(goal);
                Console.WriteLine(result);
                if (!result.HasFlag(MatchResult.Weak))
                    res = result;
                else if (result.HasFlag(MatchResult.Satisfied))
                    sat = true;

            }

            if (res.HasFlag(MatchResult.Weak))
            {
                res = sat ?
                    MatchResult.WeaklySatisfied :
                    MatchResult.WeaklyContradicted;
            }
            Console.WriteLine("Result: " + res);
            return res;
        }

        internal bool ContainsFact(Fact f)
        {
            return _state
                .OfType<Fact>()
                .Any(fact => fact.Equals(f));
        }
    }
}
