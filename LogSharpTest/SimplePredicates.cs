using Microsoft.VisualStudio.TestTools.UnitTesting;
using LogSharp;
using System.Linq;
using System;

#pragma warning disable 1718
namespace UnitTests
{
    [TestClass]
    public class SimplePredicates
    {
        World w = new World();

        // Predicates with names
        Rule red = new Rule();

        [TestInitialize]
        public void PrepWorld()
        {
            // car and fire truck are both red
            w.Add(red["car"]);
            w.Add(red["fire truck"]);
        }

        [TestMethod]
        public void Predicates()
        {
            Assert.IsTrue(w.Query(red["car"]));

            // grass is not red
            Assert.IsFalse(w.Query(red["grass"]));
        }

        [TestMethod]
        public void VariableBinding()
        {
            using (var x = new Variable())
            {
                Assert.IsTrue(w.Query(red[x]));
                Assert.IsTrue(x.Any((s) => s.Equals("car")));
                Assert.IsTrue(x.Any((s) => s.Equals("fire truck")));
            }
        }
    }
}