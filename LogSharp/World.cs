using System;
using System.Collections.Generic;
using System.Linq;

namespace LogSharp
{
    public class World
    {
        private readonly IList<ITermInternal> _state = new List<ITermInternal>();


        public World()
        {
            // since this rule can't be used by the program, it
            // literally shouldn't matter but it does ughhhh
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
        public bool Add(ITerm r)
        {
            var match = this.NonContradict(r);

            // if goal is incompatible or directly contradicted, don't add it
            if (match == MatchResult.Contradicted 
                || match == MatchResult.Incompatible)
                return false;

            // if goal is satisfied, it doesn't need to be added, but return
            // true anyway since it is part of the world.
            if (match == MatchResult.Compatible && ! (r is Rule.NegatedRule))
                _state.Add((ITermInternal) r);

            return true;
        }

        /// <summary>
        /// Evaluates whether or not a rule creates a contradiction in the world
        /// without adding it to the state of the world. Will set values of variables
        /// in the rules with all possible values.
        /// </summary>
        public bool Query(ITerm goal)
        {
            return this.NonContradict(goal) == MatchResult.Satisfied;
        }

        private MatchResult NonContradict(ITerm goal)
        {
            //var sat = false;
            //Console.WriteLine("_state: " + _state.Count());
            MatchResult res = MatchResult.Compatible;
            foreach (var f in _state)
            {
                // Console.WriteLine(goal.GetType().Name
                //     + " to "
                //     + f.GetType().Name
                //     + ": ");
                var result = f.Match((ITermInternal)goal, this);
                //Console.WriteLine(result);
                if (!result.HasFlag(MatchResult.Weak))
                    res = result;
                else if (result.HasFlag(MatchResult.Satisfied)){}
                    //sat = true;

            }

            // if (res.HasFlag(MatchResult.Weak))
            // {
            //     res = sat ?
            //         MatchResult.Compatible :
            //         MatchResult.Incompatible;
            // }
            //Console.WriteLine("Result: " + res + "\n");
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
