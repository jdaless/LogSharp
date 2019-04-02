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
        public void Variables(){
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
        public void Binding()
        {
            World w = new World();
            Rule red = new Rule();
            w.Add(red["car"]);
            w.Add(red["fire truck"]);

            // Setting variable
            var m = new Variable();
            w.Query(red[m]);
            Assert.IsTrue(m.OfType<string>().Any((s) => s.Equals("car")));
            Assert.IsTrue(m.OfType<string>().Any((s) => s.Equals("fire truck")));
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
