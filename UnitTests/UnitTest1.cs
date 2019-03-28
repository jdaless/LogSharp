using Microsoft.VisualStudio.TestTools.UnitTesting;
using LogSharp;
using System.Linq;

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
        public void RuleConstruction()
        {
            World w = new World();

            // Create a fact that is added to the world
            Fact somethingTrue = new Fact();
            w.Add(somethingTrue);

            // Create a fact that is not added to the world
            Fact somethingFalse = new Fact();
            Assert.IsTrue(w.Query(somethingTrue));
            Assert.IsFalse(w.Query(~somethingTrue));
            Assert.IsFalse(w.Query(somethingFalse));
            Assert.IsTrue(w.Query(~somethingFalse));
            Assert.IsTrue(w.Query(somethingTrue | somethingFalse));
            Assert.IsFalse(w.Query(somethingTrue & somethingFalse));
            Assert.IsTrue(w.Query(somethingTrue & somethingTrue));
        }

        [TestMethod]
        public void Implication()
        {
            World w = new World();

            Fact somethingTrue = new Fact();
            w.Add(somethingTrue);

            Fact implicans = new Fact();
            w.Add(somethingTrue > implicans);
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
        public void Socrates()
        {
            World w = new World();

            // Rules with variables
            Rule man = new Rule();
            Rule mortal = new Rule();
            using (var x = new Variable<string>())
            {
                w.Add(man[x] > mortal[x]);
            }
            w.Add(man["socrates"]);
            Assert.IsTrue(w.Query(mortal["socrates"]));
        }

        [TestMethod]
        public void Binding()
        {
            World w = new World();
            Rule red = new Rule();
            w.Add(red["car"]);
            w.Add(red["fire truck"]);

            // Setting variable
            var m = new Variable<string>();
            w.Query(red[m]);
            Assert.IsTrue(m.Any((s) => s.Equals("car")));
            Assert.IsTrue(m.Any((s) => s.Equals("fire truck")));
        }

        [TestMethod]
        public void Family()
        {
            World w = new World();

            Rule parent = new Rule();
            Rule child = new Rule();
            Rule gparent = new Rule();
            Rule sibling = new Rule();

            // a child of a parent
            using(var p = new Variable<string>())
            using(var c = new Variable<string>())
            {
                w.Add(Fact.DoubleImply(
                    child[c,p],
                    parent[p,c]));
            }

            // sibling is reflexive
            using(var s1 = new Variable<string>())
            using(var s2 = new Variable<string>())
            {
                w.Add(Fact.DoubleImply(
                    sibling[s1,s2],
                    sibling[s2,s1]));
            }


            // grandparents are parents' parents
            using(var g = new Variable<string>())
            using(var p = new Variable<string>())
            using(var c = new Variable<string>())
            {
                w.Add(Fact.DoubleImply(
                    gparent[g,c],
                    parent[p,c] ^ parent[g,p]));
            }

            // siblings share the same parent
            using(var s1 = new Variable<string>())
            using(var s2 = new Variable<string>())
            using(var p = new Variable<string>())
            {
                w.Add(Fact.DoubleImply(
                    sibling[s1,s2], 
                    parent[s1,p] ^ parent[s2,p]));
            }

            string alice = "alice";
            string bob = "bob";
            string charlie = "charlie";
            string duckie = "duckie";
            w.Add(parent[alice, bob]);
            w.Add(child[charlie, alice]);
            w.Add(parent[duckie, alice]);

            Assert.IsTrue(w.Query(gparent[duckie, bob]));
            Assert.IsTrue(w.Query(sibling[bob, charlie]));

            using(var x = new Variable<string>())
            {
                w.Query(gparent[duckie, x]);
                Assert.IsTrue(x.Any((s) => s == bob));
                Assert.IsTrue(x.Any((s) => s == charlie));
            }

        }
    }
}
