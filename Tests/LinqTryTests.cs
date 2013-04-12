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
using System.Linq;
using NUnit.Framework;
using Scando;

namespace ScandoTests {
    [TestFixture]
    public class LinqTryTests {

        public class TestException : Exception { }

        [Test]
        public void Where_on_Failure_returns_Failure() {
            var fail = new Failure<int>(new TestException());
            var where = fail.Where(x => x > 10);
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
        public void Matching_Where_on_Success_returns_Success() {
            var success = new Success<int>(42);
            var where = success.Where(x => x > 10);
            Assert.IsTrue(where.IsSuccess);
            Assert.AreEqual(42, where.Value);
        }

        [Test]
        public void Where_via_linq_syntax_on_Failure_returns_empty_enumerable() {
            var fail = new Failure<int>(new TestException());
            var where = (from x in fail where x > 10 select x);
            Assert.IsFalse(where.Any());
        }

        [Test]
        public void Non_matching_Where_via_linq_syntax_on_Success_returns_empty_enumerable() {
            var success = new Success<int>(42);
            var where = (from x in success where x < 10 select x);
            Assert.IsFalse(where.Any());
        }

        [Test]
        public void Matching_Where_via_linq_syntax_on_Success_returns_single_item_enumerable_containing_Success_value() {
            var success = new Success<int>(42);
            var where = (from x in success where x > 10 select x);
            Assert.IsTrue(where.Any());
            Assert.AreEqual(42, where.First());
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
    }
}