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
    public class ForComprehensionTests {

        [Test]
        public void Can_select_option_from_try() {
            var t = new Success<Option<int>>(Option<int>.Some(42));
            var r = from a in t
                    from b in a
                    select b;
            Assert.AreEqual(42,r.Value);
        }

        [Test]
        public void Can_select_option_from_select_of_try_option_() {
            var r = from a in Api.CanFailOrBeNull("foo")
                    from b in a
                    select b;
            Assert.AreEqual("foo",r.Value);
        }


        [Test]
        public void Can_mix_and_match_Try_and_Option() {
            var r = from a in Api.CanFail("foo") 
                    from b in Api.CanFailOrBeNull(a)
                    from c in b
                    from d in Api.CanBeNull(c)
                    select d;
            Assert.AreEqual("foo",r.Value);
        }

        [Test]
        public void Can_use_where_in_try_comprehension() {
            var r = from a in Api.CanFail("foo") where a == "foo"
                    from b in Api.CanFail(a) where b == "foo"
                    select b;
            Assert.AreEqual("foo", r.Value);
        }

        [Test]
        public void Can_use_where_in_option_comprehension() {
            var r = from a in Api.CanBeNull("foo") where a == "foo"
                    from b in Api.CanBeNull(a) where b == "foo"
                    select b;
            Assert.AreEqual("foo", r.Value);
        }

        public static class Api {

            public static Try<string> CanFail(string input) {
                return new Success<string>(input);
            }

            public static Try<string> CanFail(Exception fail) {
                return new Failure<string>(fail);
            }

            public static Option<string> CanBeNull(string input) {
                return Option<string>.Some(input);
            }

            public static Try<Option<string>> CanFailOrBeNull(string input) {
                return new Success<Option<string>>(Option<string>.Some(input));
            }

            public static Try<Option<string>> CanFailOrBeNull(Exception fail) {
                return new Failure<Option<string>>(fail);
            }
        }
    }
}