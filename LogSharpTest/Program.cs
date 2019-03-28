using LogSharp;
using System;

namespace LogSharpTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // Simple fact evaluation
            World w = new World();

            // Create a fact that is added to the world
            Fact somethingTrue = new Fact();
            w.Add(somethingTrue);

            // Create a fact that is not added to the world
            Fact somethingFalse = new Fact();

            Console.Out.WriteLine(w.Query(somethingTrue)); // True
            Console.Out.WriteLine(w.Query(~somethingTrue)); // False
            Console.Out.WriteLine(w.Query(somethingFalse)); // False
            Console.Out.WriteLine(w.Query(~somethingFalse)); // True
            Console.Out.WriteLine(w.Query(somethingTrue | somethingFalse)); // True
            Console.Out.WriteLine(w.Query(somethingTrue & somethingFalse)); // False
            Console.Out.WriteLine(w.Query(somethingTrue & somethingTrue)); // True

            // Predicates with names
            Rule red = new Rule();

            // car and fire truck are both red
            w.Add(red["car"]);
            w.Add(red["fire truck"]);
            Console.Out.WriteLine(w.Query(red["car"])); // True
            Console.Out.WriteLine(w.Query(red["grass"])); // False

            // Rules with variables
            Rule man = new Rule();
            Rule mortal = new Rule();
            using (var x = new Variable())
            {
                w.Add(man[x] > mortal[x]);
            }
            w.Add(man["socrates"]);
            Console.Out.WriteLine(w.Query(mortal["socrates"])); // True

            //// Setting variable
            //Variable m = new Variable();
            //w.Query(red[m]);
            //Console.Out.WriteLine(string.Join(", ",m.Values));

            //// Use of empty variable
            //Rule isSmallestThing = new Rule();
            //Rule isLargerThan = new Rule();
            //w.Add(isLargerThan["a", "b"]);
            //using (var x = new Variable())
            //{
            //    w.Add(isSmallestThing[x] <
            //        ~isLargerThan[x, Variable._]);
            //}
            //Console.Out.WriteLine(w.Query(isSmallestThing["a"]));
            //Console.Out.WriteLine(w.Query(isSmallestThing["b"]));


            Console.In.Read();

        }
    }
}
