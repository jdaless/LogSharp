using System;
using System.Collections.Generic;
using System.Text;

namespace LogSharp
{
    public interface ITerm
    {
        Rule Not();

        /// <summary>
        /// Right implication, the rule is satisfied if the right argument 
        /// follows from the left one.
        /// </summary>
        /// <param name="r1">Implicator</param>
        /// <param name="r2">Implicans</param>
        /// <returns>A new rule representing that r1 implies r2</returns>
        Rule Implies(ITerm t2);

        
        /// <summary>
        /// Left implication, the rule is satisfied if the left argument 
        /// follows from the right one.
        /// </summary>
        /// <param name="r1">Implicator</param>
        /// <param name="r2">Implicans</param>
        /// <returns>A new rule representing that r1 implies r2</returns>
        Rule If(ITerm t2);
        Rule Conjoin(ITerm t2);
        Rule Disjoin(ITerm t2);
        Rule IFF(ITerm t2);
    }

    internal interface ITermInternal : ITerm
    {
        MatchResult Match(ITermInternal goal, World w);
        bool VariablesSatisfied();
    }

    [FlagsAttribute]
    internal enum MatchResult
    {
        // Definitely true in the domain
        Satisfied = 0b10,
        // Definitely false in the domain
        Contradicted = 0b00,
        
        Weak = 0b01,
        // Could be true in the domain
        Compatible = 0b11,
        // Could be false in the domain
        Incompatible = 0b01
    }
}
