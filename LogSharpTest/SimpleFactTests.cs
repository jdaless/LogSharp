using Microsoft.VisualStudio.TestTools.UnitTesting;
using LogSharp;
using System.Linq;
using System;

#pragma warning disable 1718
namespace UnitTests
{
    [TestClass]
    public class SimpleFactTests
    {
        World w = new World();
        Fact p = new Fact();

        [TestInitialize]
        public void PrepWorld()
        {
            Assert.IsTrue(w.Add(p));
        }

        [TestMethod]
        public void SimpleFactEvaluation()
        {
            Assert.IsTrue(w.Query(p));
        }

        [TestMethod]
        public void Contradiction()
        {
            Assert.IsFalse(w.Add(~p));
        }

    }
}
