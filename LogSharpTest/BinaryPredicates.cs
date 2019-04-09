using Microsoft.VisualStudio.TestTools.UnitTesting;
using LogSharp;
using System.Linq;
using System;

#pragma warning disable 1718
namespace UnitTests
{
    [TestClass]
    public class BinaryPredicates
    {

        [TestMethod]
        public void Relationships()
        {
            World w = new World();
            Rule likes = new Rule();
            w.Add(likes["mary", "food"]);
            w.Add(likes["mary", "wine"]);
            w.Add(likes["john", "wine"]);
            w.Add(likes["john", "mary"]);
            Assert.IsTrue(w.Query(likes["mary", "food"]));
            Assert.IsTrue(w.Query(likes["john", "wine"]));
            Assert.IsFalse(w.Query(likes["john", "food"]));
        }

        [TestMethod]
        public void RuleBinding()
        {
            World w = new World();
            Rule likes = new Rule();

            // Alice likes anyone
            w.Add(likes["alice", Variable._]);
            using (var x = new Variable())
            {
                // who likes bob?
                Assert.IsTrue(w.Query(likes[x, "bob"]));

                // alice does!
                Assert.IsTrue(x.Any((s) => s.Equals("alice")));
            }
        }

        [TestMethod]
        public void MixingTypes()
        {
            World w = new World();
            Rule isCalled = new Rule();

            w.Add(isCalled[9, "nine"]);
            w.Add(isCalled["IX", "nine"]);
            w.Add(isCalled[9.0, "nine"]);

            using (var x = new Variable())
            {
                Assert.IsTrue(w.Query(isCalled[x, "nine"]));

                Assert.IsTrue(x.Any((s) => s.Equals(9)));
                Assert.IsTrue(x.Any((s) => s.Equals("IX")));
                Assert.IsTrue(x.Any((s) => s.Equals(9.0)));
            }
        }
    }
}
