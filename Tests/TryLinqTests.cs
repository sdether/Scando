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
    public class TryLinqTests {

        [Test]
        public void Can_use_linq_comprehension_on_Try() {
            var success = new Success<int>(42);
            var q = (from x in success select x);
            Assert.AreEqual(42, q.Value);
        }

        [Test]
        public void Where_on_Failure_returns_Failure() {
            var fail = new Failure<int>(new TestException());
            var where = fail.Where(x => x > 10);
            Assert.IsTrue(where.IsFailure);
            Assert.IsInstanceOfType(typeof(TestException), where.Exception);
        }

        [Test]
        public void Where_via_linq_comprehension_on_Failure_returns_empty_enumerable() {
            var fail = new Failure<int>(new TestException());
            var where = (from x in fail where x > 10 select x);
            Assert.IsTrue(where.IsFailure);
            Assert.IsInstanceOfType(typeof(TestException), where.Exception);
        }

        [Test]
        public void Non_matching_Where_on_Success_return_Failure() {
            var success = new Success<int>(42);
            var where = success.Where(x => x < 10);
            Assert.IsTrue(where.IsFailure);
            Assert.IsInstanceOfType(typeof(NoSuchElementException), where.Exception);
        }

        [Test]
        public void Non_matching_Where_via_linq_comprehension_on_Success_returns_empty_enumerable() {
            var success = new Success<int>(42);
            var where = (from x in success where x < 10 select x);
            Assert.IsTrue(where.IsFailure);
            Assert.IsInstanceOfType(typeof(NoSuchElementException), where.Exception);
        }

        [Test]
        public void Matching_Where_on_Success_returns_Success() {
            var success = new Success<int>(42);
            var where = success.Where(x => x > 10);
            Assert.IsTrue(where.IsSuccess);
            Assert.AreEqual(42, where.Value);
        }

        [Test]
        public void Matching_Where_via_linq_comprehension_on_Success_returns_single_item_enumerable_containing_Success_value() {
            var success = new Success<int>(42);
            var where = (from x in success where x > 10 select x);
            Assert.IsTrue(where.IsSuccess);
            Assert.AreEqual(42, where.Value);
        }

        [Test]
        public void ToTry_converts_empty_enumerable_into_NoSuchElement_Try() {
            var x = new int[0];
            var t = x.ToTry();
            Assert.IsTrue(t.IsFailure);
            Assert.IsInstanceOfType(typeof(NoSuchElementException), t.Exception);
        }

        [Test]
        public void ToTry_converts_non_empty_enumerable_into_Try_containing_the_first_value() {
            var x = new[] { 42, 99 };
            var t = x.ToTry();
            Assert.IsTrue(t.IsSuccess);
            Assert.AreEqual(42, t.Value);
        }

        [Test]
        public void Chaining_successes_return_last_success() {
            var r = Api.SuccessInt(10).SelectMany(x => Api.SuccessString(x.ToString())).SelectMany(Api.SuccessString);
            Assert.IsTrue(r.IsSuccess);
            Assert.AreEqual("20.20.20.20", r.Value);
        }

        [Test]
        public void Chaining_successes_via_linq_comprehension_returns_the_last_success() {
            var r =
                from a in Api.SuccessInt(10)
                from b in Api.SuccessString(a.ToString())
                from c in Api.SuccessString(b)
                select c;
            Assert.IsTrue(r.IsSuccess);
            Assert.AreEqual("20.20.20.20", r.Value);
        }

        [Test]
        public void Chaining_trys_with_failures_returns_first_failure() {
            var r = Api.SuccessString("a").SelectMany(_ => Api.FailString(new TestException("x"))).SelectMany(_ => Api.FailInt(new TestException("y"))).SelectMany(Api.SuccessInt);
            Assert.IsTrue(r.IsFailure);
            Assert.AreEqual("x", r.Exception.Message);
        }

        [Test]
        public void Chaining_trys_via_linq_comprehension_returns_first_failure() {
            var r = from a in Api.SuccessString("a")
                    from b in Api.FailString(new TestException("x"))
                    from c in Api.FailInt(new TestException("y"))
                    from d in Api.SuccessInt(c)
                    select d;
            Assert.IsTrue(r.IsFailure);
            Assert.AreEqual("x", r.Exception.Message);
        }

        [Test]
        public void Can_use_OrElse_in_linq_comprehension() {
            var r = from a in Api.FailString(new TestException("x")).OrElse(new Success<string>("psych"))
                    from b in Api.SuccessString(a)
                    select b;
            Assert.IsTrue(r.IsSuccess);
            Assert.AreEqual("psych.psych",r.Value);
        }

        private static class Api {
            public static Try<string> SuccessString(string input) {
                return new Success<string>(input + "." + input);
            }

            public static Try<string> FailString(Exception exception) {
                return new Failure<string>(exception);
            }

            public static Try<int> SuccessInt(int input) {
                return new Success<int>(input + input);
            }

            public static Try<int> FailInt(Exception exception) {
                return new Failure<int>(exception);
            }
        }

        public class TestException : Exception {
            public TestException() {}
            public TestException(string msg) : base(msg) { }
        }

    }
}