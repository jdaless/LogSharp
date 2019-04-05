using System;
using System.Collections.Generic;
using System.Text;

namespace LogSharp
{
    public interface IFact
    {
    }

    internal interface IFactInternal : IFact
    {
        MatchResult Match(IFactInternal goal, World w);
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
