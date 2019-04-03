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
            // this.Add(new Rule()["red"]);
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
            if(this.NonContradict(r) == MatchResult.Contradicted) 
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

            return this.NonContradict(goal) == MatchResult.Satisfied;
        }

        private MatchResult NonContradict(IFact goal)
        {
            var satisfied = false;
            foreach (var f in _state)
            {
                var result = f.Match(goal, this);
                switch(result)
                {
                    case MatchResult.Contradicted:
                        return MatchResult.Contradicted;
                    case MatchResult.Satisfied:
                        satisfied = true;
                        break;
                }
            }
            
            return (satisfied)?MatchResult.Satisfied:MatchResult.Inconclusive;
        }

        internal bool ContainsFact(Fact f)
        {
            return _state
                .OfType<Fact>()
                .Any(fact => fact.Equals(f));
        }
    }
}
