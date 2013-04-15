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
using System.Linq;
using NUnit.Framework;
using Scando;

namespace ScandoTests {

    [TestFixture]
    public class OptionLinqTests {

        [Test]
        public void Can_use_linq_comprehension_on_Option() {
            var success = Option<int>.Some(42);
            var q = (from x in success select x);
            Assert.AreEqual(42, q.Value);
        }

        [Test]
        public void Can_use_linq_comprehension_on_Option_from_a_method_call() {
            var q = (from x in Api.DoSomething() select x);
            Assert.AreEqual("foo", q.Value);
        }

        [Test]
        public void Can_convert_IEnumerable_with_values_to_Option() {
            var enumerable = new[] { 123 };
            var o = enumerable.ToOption();
            Assert.AreEqual(123, o.Value);
        }

        [Test]
        public void Can_convert_empty_IEnumerable_to_Option() {
            var enumerable = new int[0];
            var o = enumerable.ToOption();
            Assert.AreEqual(Option<int>.None, o);
        }

        [Test]
        public void Can_chain_somes() {
            var r = Api.DoSomething().SelectMany(Api.Passthrough).SelectMany(Api.Passthrough).GetOrElse("bar");
            Assert.AreEqual("foo", r);
        }

        [Test]
        public void Can_chain_nones() {
            var r = Api.DoNothing().SelectMany(Api.DoNothing).GetOrElse(42);
            Assert.AreEqual(42, r);
        }

        [Test]
        public void Can_chain_with_none_in_the_middle() {
            var r = Api.DoSomething().SelectMany(Api.DoNothing).SelectMany(Api.Passthrough).GetOrElse(42);
            Assert.AreEqual(42, r);
        }

        [Test]
        public void Chaining_a_none_will_continue_to_return_none() {
            var r = Api.DoNothing().SelectMany(Api.DoSomething).GetOrElse(42);
            Assert.AreEqual(42, r);
        }

        [Test]
        public void Chaining_with_the_last_being_a_none_returns_none() {
            var r = Api.DoSomething().SelectMany(Api.DoNothing).GetOrElse(42);
            Assert.AreEqual(42, r);
        }

        [Test]
        public void Can_chain_via_linq_comprehension() {
            var r = (from a in Api.Double(2)
                     from b in Api.Double(a)
                     from c in Api.Double(b)
                     select c);
            Assert.AreEqual(16, r.Value);
        }

        [Test]
        public void Linq_comprehension_with_None_in_chain_returns_none() {
            var r = (from a in Api.Double(2)
                     from b in Api.DoNothing(a)
                     from c in Api.Double(b)
                     select c);
            Assert.IsFalse(r.IsDefined);
        }

        [Test]
        public void Can_use_orElse_in_linq_comprehension() {
            var r = from a in Api.Double(2)
                    from b in Api.DoNothing(a).OrElse(Option<int>.Some(42))
                    from c in Api.Double(b)
                    select c;
            Assert.IsTrue(r.IsDefined);
            Assert.AreEqual(84, r.Value);
        }

        [Test]
        public void Can_convert_linq_result_to_Option() {
            var o = (from a in new[] { "foo" } select a + "bar").ToOption();
            Assert.IsTrue(o.IsDefined);
            Assert.AreEqual("foobar", o.Value);
        }

        [Test]
        public void Can_convert_empty_linq_result_to_Option() {
            var o = (from a in new string[0] select a + "var").ToOption();
            Assert.AreEqual(Option<string>.None, o);
        }

        private static class Api {
            public static Option<string> DoNothing() {
                return Option<string>.None;
            }

            public static Option<string> DoSomething() {
                return Option<string>.Some("foo");
            }

            public static Option<int> DoNothing(string input) {
                return Option<int>.None;
            }

            public static Option<int> DoSomething(string input) {
                return Option<int>.Some(45);
            }

            public static Option<string> Passthrough(string v) {
                return Option<string>.Some(v);
            }
            public static Option<int> Passthrough(int v) {
                return Option<int>.Some(v);
            }

            public static Option<int> Double(int x) {
                return Option<int>.Some(x + x);
            }

            public static Option<int> DoNothing(int x) {
                return Option<int>.None;
            }
        }
    }
}