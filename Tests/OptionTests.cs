/*-----------------------------------------------------------------------------
 * Copyright (c) 2013 Arne F. Claassen.
 *
 * Permission is hereby granted, free of charge, to any person obtaining
 * a copy of this software and associated documentation files (the
 * "Software"), to deal in the Software without restriction, including
 * without limitation the rights to use, copy, modify, merge, publish,
 * distribute, sublicense, and/or sell copies of the Software, and to
 * permit persons to whom the Software is furnished to do so, subject to
 * the following conditions:
 * 
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
 * LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
 * OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
 * WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 *---------------------------------------------------------------------------*/
using System;
using NUnit.Framework;
using Scando;

namespace ScandoTests {
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
