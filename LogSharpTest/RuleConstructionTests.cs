using Microsoft.VisualStudio.TestTools.UnitTesting;
using LogSharp;
using System.Linq;
using System;

#pragma warning disable 1718
namespace UnitTests
{
    [TestClass]
    public class RuleConstructionTests
    {
        World w = new World();
        Fact somethingTrue = new Fact();

        Fact somethingFalse = new Fact();

        Fact somethingElse = new Fact();

        [TestInitialize]
        public void PrepWorld()
        {
            // somethingTrue added to the world
            w.Add(somethingTrue);
            // somethingFalse's negation is added to the world
            w.Add(!somethingFalse);
            // somethingElse is not represented in the world at all
        }
        [TestMethod]
        public void Negation()
        {
            Assert.IsTrue(w.Query(somethingTrue));
            Assert.IsFalse(w.Query(!somethingTrue));
            Assert.IsTrue(w.Query(!!somethingTrue));

            Assert.IsFalse(w.Query(somethingFalse));
            Assert.IsTrue(w.Query(!somethingFalse));
            Assert.IsFalse(w.Query(!!somethingFalse));

            Assert.IsFalse(w.Query(somethingElse));
            Assert.IsTrue(w.Query(!somethingElse));
            Assert.IsFalse(w.Query(!!somethingElse));
        }

        [TestMethod]
        public void Conjunction()
        {
            // Truth table for somethingTrue and somethingFalse
            Assert.IsTrue(w.Query(somethingTrue & somethingTrue));
            Assert.IsFalse(w.Query(somethingTrue & somethingFalse));
            Assert.IsFalse(w.Query(somethingFalse & somethingTrue));
            Assert.IsFalse(w.Query(somethingFalse & somethingFalse));

            // Truth table for somethingTrue and somethingElse
            Assert.IsTrue(w.Query(somethingTrue & somethingTrue));
            Assert.IsFalse(w.Query(somethingTrue & somethingElse));
            Assert.IsFalse(w.Query(somethingElse & somethingTrue));
            Assert.IsFalse(w.Query(somethingElse & somethingElse));
        }

        [TestMethod]
        public void Disjunction()
        {
            // Truth table for somethingTrue and somethingFalse
            Assert.IsTrue(w.Query(somethingTrue | somethingTrue));
            Assert.IsTrue(w.Query(somethingTrue | somethingFalse));
            Assert.IsTrue(w.Query(somethingFalse | somethingTrue));
            Assert.IsFalse(w.Query(somethingFalse | somethingFalse));
            
            // Truth table for somethingTrue and somethingElse
            Assert.IsTrue(w.Query(somethingTrue | somethingTrue));
            Assert.IsTrue(w.Query(somethingTrue | somethingElse));
            Assert.IsTrue(w.Query(somethingElse | somethingTrue));
            Assert.IsFalse(w.Query(somethingElse | somethingElse));
            
            // Truth table for somethingFalse and somethingElse
            Assert.IsFalse(w.Query(somethingFalse | somethingFalse));
            Assert.IsFalse(w.Query(somethingFalse | somethingElse));
            Assert.IsFalse(w.Query(somethingElse | somethingFalse));
            Assert.IsFalse(w.Query(somethingElse | somethingElse));
        }

        [TestMethod]
        public void Implication()
        {
            Assert.IsTrue(w.Query(somethingTrue > somethingTrue));
            Assert.IsFalse(w.Query(somethingTrue > somethingFalse));
            Assert.IsTrue(w.Query(somethingFalse > somethingTrue));
            Assert.IsTrue(w.Query(somethingFalse > somethingFalse));

            Assert.IsTrue(w.Query(somethingTrue < somethingTrue));
            Assert.IsTrue(w.Query(somethingTrue < somethingFalse));
            Assert.IsFalse(w.Query(somethingFalse < somethingTrue));
            Assert.IsTrue(w.Query(somethingFalse < somethingFalse));
        }
    }
}
