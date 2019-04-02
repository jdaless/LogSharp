using System;
using System.Collections.Generic;
using System.Linq;

namespace LogSharp
{
    public class World
    {
        private readonly IList<IFact> _state = new List<IFact>();

        /// <summary>
        /// Adds a new rule to the state of the world. Returns true on success,
        /// returns false and fails to add the rule if the new rule would 
        /// create a contradiction.
        /// </summary>
        public bool Add(IFact r)
        {
            foreach (IFact f in _state){
                // check to see if r contradicts f
                // return false if so
            }
            _state.Add(r);
             return true;
        }

        /// <summary>
        /// Evaluates whether or not a rule creates a contradiction in the world
        /// without adding it to the state of the world. Will set values of variables
        /// in the rules with all possible values.
        /// </summary>
        public bool Query(IFact fact)
        {         
            var satisfied = false;
            foreach (var f in _state)
            {
                var result = f.Match(fact, this);
                switch(result)
                {
                    case MatchResult.Contradicted:
                        return false;
                    case MatchResult.Satisfied:
                        satisfied = true;
                        break;
                }
            }
            return satisfied;
        }

        internal bool ContainsFact(Fact f)
        {
            return _state
                .OfType<Fact>()
                .Any(fact => fact.Equals(f));
        }
    }
}
