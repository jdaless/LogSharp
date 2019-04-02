using System;
using System.Collections.Generic;
using System.Text;

namespace LogSharp
{
    public interface IFact
    {
        bool Evaluate(World w);
        bool Coerce(World w);
        MatchResult Match(IFact goal, World w);
    }

    public enum MatchResult
    {
        Satisfied,
        Inconclusive,
        Contradicted
    }
}
