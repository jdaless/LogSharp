using Microsoft.VisualStudio.TestTools.UnitTesting;
using LogSharp;
using System.Linq;
using System;

#pragma warning disable 1718
namespace UnitTests
{
    [TestClass]
    public class FactDerivations
    {
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
        public void ModusTollens()
        {
            World w = new World();

            Fact implicator = new Fact();

            Fact somethingFalse = new Fact();

            w.Add(implicator > somethingFalse);

            // somethingFalse was not added to the world, so
            // we know that implicator is not true
            Assert.IsFalse(w.Query(implicator));
            Assert.IsTrue(w.Query(~implicator));
        }

        [TestMethod]
        public void ComplexModusPonens()
        {
            World w = new World();

            Fact p = new Fact();
            w.Add(p);
            Fact q = new Fact();
            w.Add(q);
            Fact r = new Fact();
            w.Add(r);

            Fact implicans = new Fact();
            w.Add(implicans < (p ^ q ^ r));

            // implicans was not added to the world, but is
            // a consequence of the rules in the world.
            Assert.IsTrue(w.Query(implicans));

        }
    }
}
