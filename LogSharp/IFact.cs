using System;
using System.Collections.Generic;
using System.Text;

namespace LogSharp
{
    public interface IFact
    {
        MatchResult Match(IFact goal);
        bool VariablesSatisfied();
    }

    [FlagsAttribute]
    public enum MatchResult
    {
        // Definitely true in the domain
        Satisfied = 0b10,
        // Definitely false in the domain
        Contradicted = 0b00,
        
        Weak = 0b01,
        // Could be true in the domain
        WeaklySatisfied = 0b11,
        // Could be false in the domain
        WeaklyContradicted = 0b01
    }
}
