using System;
using NUnit.Framework;
using Option;

namespace Tests {

    [TestFixture]
    public class OptionTests {

        [Test]
        public void Creating_Option_with_null_value_returns_none() {

            var x = Option<string>.Some(null);

            Assert.AreSame(Option<String>.None, x);
        }

        [Test]
        public void Creating_Option_with_null_for_nullable_value_type_returns_none() {
            var x = Option<int?>.Some(null);

            Assert.AreSame(Option<int?>.None, x);
        }

        [Test]
        public void Get_gets_value_of_Some() {
            var x = Option<string>.Some("bob");
            Assert.AreEqual("bob", x.Get());
        }

        [Test, ExpectedException(typeof(InvalidOperationException))]
        public void Get_throws_InvalidOperationExection_for_None() {
            var x = Option<string>.None;
            x.Get();
        }

        [Test]
        public void GetOrElse_gets_value_of_Some() {
            var x = Option<string>.Some("bob");
            Assert.AreEqual("bob", x.GetOrElse("jane"));
        }

        [Test]
        public void GetOrElse_gets_default_for_None() {
            var x = Option<string>.None;
            Assert.AreEqual("jane", x.GetOrElse("jane"));
        }

        [Test]
        public void Some_is_defined() {
            var x = Option<string>.Some("bob");
            Assert.IsTrue(x.IsDefined);
        }

        [Test]
        public void None_is_not_defined() {
            var x = Option<string>.None;
            Assert.IsFalse(x.IsDefined);
        }

        [Test]
        public void OrElse_gets_current_Option_for_Some() {
            var x = Option<string>.Some("bob");
            var y = Option<string>.Some("jane");
            Assert.AreSame(x, x.OrElse(y));
        }

        [Test]
        public void OrElse_gets_alternative_Option_for_None() {
            var x = Option<string>.None;
            var y = Option<string>.Some("jane");
            Assert.AreSame(y, x.OrElse(y));
        }

        [Test]
        public void Option_equality_compares_against_captured_value() {
            Assert.IsTrue(Option<string>.Some("a").Equals("a"));
        }

        [Test]
        public void None_equals_null() {
            Assert.IsTrue(Option<string>.None.Equals(null));
        }

        [Test]
        public void None_of_different_types_are_equal() {
            Assert.IsTrue(Option<string>.None.Equals(Option<int>.None));
        }

        [Test]
        public void Option_of_a_has_same_hashcode_as_a() {
            Assert.AreEqual("a".GetHashCode(), Option<string>.Some("a").GetHashCode());
        }

        [Test]
        public void None_of_different_types_have_same_hashcode() {
            Assert.AreEqual(
                Option<string>.None.GetHashCode(),
                Option<int>.None.GetHashCode()
            );
        }
    }
}
