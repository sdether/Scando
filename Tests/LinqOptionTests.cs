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
    }
}