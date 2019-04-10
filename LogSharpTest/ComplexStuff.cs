using Microsoft.VisualStudio.TestTools.UnitTesting;
using LogSharp;
using System.Linq;
using System;

#pragma warning disable 1718
namespace UnitTests
{
    [TestClass]
    public class ComplexStuff
    {
        [TestMethod]
        public void BuiltInPredicates()
        {
            World w = new World();
            Assert.IsTrue(w.Query(Predicate.Equality[1, 1]));
            using (var x = new Variable())
            {
                w.Query(Predicate.Equality[x, 5]);
                Assert.AreEqual(x.First(), 5);
            }
        }

        [TestMethod]
        public void Socrates()
        {
            World w = new World();

            // Rules with variables
            Predicate man = new Predicate();
            Predicate mortal = new Predicate();
            using (var x = new Variable())
            {
                w.Add(man[x] > mortal[x]);
            }
            w.Add(man["socrates"]);
            using (var x = new Variable())
            {
                Assert.IsTrue(w.Query(mortal[x]));
                Assert.IsTrue(x.First().Equals("socrates"));
            }
            Assert.IsTrue(w.Query(mortal["socrates"]));
        }

        [TestMethod]
        public void Abraham()
        {
            World w = new World();
            Predicate son = new Predicate();
            Predicate patriarch = new Predicate();
            Predicate freeborn = new Predicate();
            w.Add(son["ishmael", "abraham", "mother_is_slave"]);
            w.Add(son["isaac", "abraham", "mother_is_free"]);
            w.Add(patriarch["abraham"]);
            using (var p = new Variable())
            using (var s = new Variable())
            using (var f = new Variable())
            {
                w.Add(freeborn[p, s] < (
                            patriarch[p] ^
                            son[s, p, f] ^
                            Predicate.Equality[f, "mother_is_free"])
                        );
            }
            using (var x = new Variable())
            using (var y = new Variable())
            {
                w.Query(freeborn[x, y]);
                Assert.IsTrue(x.Single().Equals("abraham"));
                Assert.IsTrue(y.Single().Equals("isaac"));
            }
        }        
    }
}