using System.Linq;
using NUnit.Framework;
using Option;

namespace Tests {

    [TestFixture]
    public class LinqOptionTests {

        [Test]
        public void None_is_an_empty_enumerable() {
            var none = Option<string>.None.ToArray();

            Assert.AreEqual(new string[0], none);
        }

        [Test]
        public void Option_with_value_is_a_single_element_array() {
            var some = Option<string>.Some("foo").ToArray();

            Assert.AreEqual(new[] { "foo" }, some);
        }

        [Test]
        public void Can_use_Any_to_test_for_Option_none() {

            Assert.IsFalse(Option<int>.None.Any());
        }

        [Test]
        public void Can_use_Any_to_test_for_Option_value() {
            Assert.IsTrue(Option<int>.Some(42).Any());
        }

        [Test]
        public void Can_use_FirstOrDefault_to_provide_default_for_None() {
            Assert.AreEqual(0, Option<int>.None.FirstOrDefault());
        }

        [Test]
        public void Can_use_FirstOrDefault_to_get_value_of_Option() {
            Assert.AreEqual(42, Option<int>.Some(42).FirstOrDefault());
        }

        [Test]
        public void Can_chain_somes() {
            var c = new Chainable();
            var r = c.DoSomething().Select(c.Passthrough).Select(c.Passthrough).GetOrElse("bar");
            Assert.AreEqual("foo", r);
        }

        [Test]
        public void Can_chain_nones() {
            var c = new Chainable();
            var r = c.DoNothing().Select(c.DoNothing).GetOrElse(42);
            Assert.AreEqual(42, r);
        }

        [Test]
        public void Can_chain_with_none_in_the_middle() {
            var c = new Chainable();
            var r = c.DoSomething().Select(c.DoNothing).Select(c.Passthrough).GetOrElse(42);
            Assert.AreEqual(42, r);
        }

        [Test]
        public void Chaining_a_none_will_continue_to_return_none() {
            var c = new Chainable();
            var r = c.DoNothing().Select(c.DoSomething).GetOrElse(42);
            Assert.AreEqual(42, r);
        }

        [Test]
        public void Chaining_with_the_last_being_a_none_returns_none() {
            var c = new Chainable();
            var r = c.DoSomething().Select(c.DoNothing).GetOrElse(42);
            Assert.AreEqual(42, r);
        }

        [Test]
        public void Can_chain_via_from_syntax() {
            var chain = new Chainable();
            var r = (from a in chain.Double(2)
                     from b in chain.Double(a)
                     from c in chain.Double(b)
                     select c).ToList();
            Assert.IsTrue(r.Any());
            Assert.AreEqual(16, r.First());

        }

        [Test]
        public void Can_chain_via_from_syntax_with_none_in_chain() {
            var chain = new Chainable();
            var r = (from a in chain.Double(2)
                     from b in chain.DoNothing(a)
                     from c in chain.Double(b)
                     select c).ToList();
            Assert.IsFalse(r.Any());
        }

        private class Chainable {
            public Option<string> DoNothing() {
                return Option<string>.None;
            }

            public Option<string> DoSomething() {
                return Option<string>.Some("foo");
            }

            public Option<int> DoNothing(string input) {
                return Option<int>.None;
            }

            public Option<int> DoSomething(string input) {
                return Option<int>.Some(45);
            }

            public Option<string> Passthrough(string v) {
                return Option<string>.Some(v);
            }
            public Option<int> Passthrough(int v) {
                return Option<int>.Some(v);
            }

            public Option<int> Double(int x) {
                return Option<int>.Some(x + x);
            }

            public Option<int> DoNothing(int x) {
                return Option<int>.None;
            }
        }
    }
}