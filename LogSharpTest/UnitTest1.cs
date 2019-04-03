using Microsoft.VisualStudio.TestTools.UnitTesting;
using LogSharp;
using System.Linq;
using System;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void SimpleFactEvaluation()
        {
            World w = new World();
            Fact p = new Fact();
            w.Add(p);
            Assert.IsTrue(w.Query(p));
        }
        
        [TestMethod]
        public void Contradiction()
        {
            World w = new World();
            Fact p = new Fact();
            Assert.IsTrue(w.Add(p));
            Assert.IsFalse(w.Add(~p));
        }

        [TestMethod]
        public void Negation()
        {
            World w = new World();

            // Create a fact that is added to the world
            Fact somethingTrue = new Fact();
            w.Add(somethingTrue);

            // Create a fact that is not added to the world
            Fact somethingFalse = new Fact();

            Assert.IsTrue(w.Query(somethingTrue));
            Assert.IsFalse(w.Query(somethingFalse));

            Assert.IsFalse(w.Query(!somethingTrue));
            Assert.IsTrue(w.Query(!somethingFalse));

            Assert.IsTrue(w.Query(!!somethingTrue));
        }

        [TestMethod]
        public void Conjunction()
        {
            World w = new World();

            // Create a fact that is added to the world
            Fact somethingTrue = new Fact();
            w.Add(somethingTrue);

            // Create a fact that is not added to the world
            Fact somethingFalse = new Fact();

            Assert.IsTrue(w.Query(somethingTrue & somethingTrue));
            Assert.IsFalse(w.Query(somethingTrue & somethingFalse));
            Assert.IsFalse(w.Query(somethingFalse & somethingTrue));
            Assert.IsFalse(w.Query(somethingFalse & somethingFalse));
        }

        [TestMethod]
        public void Disjunction()
        {
            World w = new World();

            // Create a fact that is added to the world
            Fact somethingTrue = new Fact();
            w.Add(somethingTrue);

            // Create a fact that is not added to the world
            Fact somethingFalse = new Fact();

            Assert.IsTrue(w.Query(somethingTrue | somethingTrue));
            Assert.IsTrue(w.Query(somethingTrue | somethingFalse));
            Assert.IsTrue(w.Query(somethingFalse | somethingTrue));
            Assert.IsFalse(w.Query(somethingFalse | somethingFalse));
        }

        [TestMethod]
        public void Implication()
        {
            World w = new World();

            // Create a fact that is added to the world
            Fact somethingTrue = new Fact();
            w.Add(somethingTrue);

            // Create a fact that is not added to the world
            Fact somethingFalse = new Fact();

            Assert.IsTrue(w.Query(somethingTrue > somethingTrue));
            Assert.IsFalse(w.Query(somethingTrue > somethingFalse));
            Assert.IsTrue(w.Query(somethingFalse > somethingTrue));
            Assert.IsTrue(w.Query(somethingFalse > somethingFalse));
        }

        [TestMethod]
        public void ModusPonens()
        {
            World w = new World();

            Fact somethingTrue = new Fact();
            w.Add(somethingTrue);

            Fact implicans = new Fact();
            w.Add(somethingTrue > implicans);

            // implicans was not added to the world, but is
            // a consequence of the rules in the world.
            Assert.IsTrue(w.Query(implicans));
        }

        [TestMethod]
        public void Predicates()
        {
            World w = new World();

            // Predicates with names
            Rule red = new Rule();

            // car and fire truck are both red
            w.Add(red["car"]);
            w.Add(red["fire truck"]);
            Assert.IsTrue(w.Query(red["car"])); 

            // grass is not red
            Assert.IsFalse(w.Query(red["grass"]));
        }

        [TestMethod]
        public void BuiltInPredicates()
        {
            World w = new World();
            Assert.IsTrue(w.Query(Rule.Equality[1, 1]));
            using (var x = new Variable())
            {
                w.Query(Rule.Equality[x, 5]);
                Assert.AreEqual(x.OfType<int>().First(), 5);
            }
        }

        [TestMethod]
        public void Relationships()
        {
            World w = new World();
            Rule likes = new Rule();
            w.Add(likes["mary", "food"]);
            w.Add(likes["mary", "wine"]);
            w.Add(likes["john", "wine"]);
            w.Add(likes["john", "mary"]);
            Assert.IsTrue(w.Query(likes["mary","food"]));
            Assert.IsTrue(w.Query(likes["john","wine"]));
            Assert.IsFalse(w.Query(likes["john","food"]));
        }

        [TestMethod]
        public void VariableBinding(){
            World w = new World();
            Rule red = new Rule();
            w.Add(red["car"]);
            w.Add(red["fire truck"]);
            using(var x = new Variable()){
                Assert.IsTrue(w.Query(red[x]));
                Assert.IsTrue(x.OfType<string>().Any((s) => s.Equals("car")));
                Assert.IsTrue(x.OfType<string>().Any((s) => s.Equals("fire truck")));
            }
        }

        [TestMethod]
        public void Socrates()
        {
            World w = new World();

            // Rules with variables
            Rule man = new Rule();
            Rule mortal = new Rule();
            using (var x = new Variable())
            {
                w.Add(man[x] > mortal[x]);
            }
            w.Add(man["socrates"]);
            Assert.IsTrue(w.Query(mortal["socrates"]));
        }

        [TestMethod]
        public void Abraham()
        {
            World w = new World();
            Rule son = new Rule();
            Rule patriarch = new Rule();
            Rule freeborn = new Rule();
            w.Add(son["ishmael", "abraham", "mother_is_slave"]);
            w.Add(son["isaac", "abraham", "mother_is_free"]);
            w.Add(patriarch["abraham"]);
            using(var p = new Variable())
            using(var s = new Variable())
            using(var f = new Variable())
            {
                w.Add(freeborn[p,s] < 
                    patriarch[p] ^ 
                    son[s,p,f] ^ 
                    Rule.Equality[f, "mother_is_free"]);
            }
            using(var x = new Variable())
            using(var y = new Variable())
            {
                w.Query(freeborn[x,y]);
                Assert.IsTrue(x.OfType<string>().Single().Equals("abraham"));
                Assert.IsTrue(y.OfType<string>().Single().Equals("isaac"));
            }

        }
    }
}
