using System;
using System.Collections.Generic;
using System.Text;

namespace LogSharp
{
    public interface IFact
    {
        bool Evaluate(World w);
        bool Coerce(World w);
    }
}
