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
    public class TryTests {

        [Test]
        public void Can_create_Success_via_Try_Eval() {
            var t = Try<string>.Exec(() => "foo");
            Assert.IsTrue(t.IsSuccess);
            Assert.IsFalse(t.IsFailure);
            Assert.AreEqual("foo", t.Value);
        }

        [Test]
        public void Can_create_Success() {
            var t = new Success<string>("foo");
            Assert.IsTrue(t.IsSuccess);
            Assert.IsFalse(t.IsFailure);
            Assert.AreEqual("foo", t.Value);
        }

        [Test]
        public void Can_create_Failure_via_Try_Eval() {
            var t = Try<string>.Exec(() => { throw new Exception("fail"); });
            Assert.IsFalse(t.IsSuccess);
            Assert.IsTrue(t.IsFailure);
            Assert.AreEqual("fail", t.Exception.Message);
        }

        [Test]
        public void Can_create_Failure() {
            var t = new Failure<string>(new Exception("fail"));
            Assert.IsFalse(t.IsSuccess);
            Assert.IsTrue(t.IsFailure);
            Assert.AreEqual("fail", t.Exception.Message);
        }

        [Test]
        public void GetOrElse_on_Success_returns_Success() {
            var t = new Success<string>("foo");
            Assert.AreEqual("foo", t.GetOrElse("bar"));
        }

        [Test]
        public void GetOrElse_on_Failure_returns_else() {
            var t = new Failure<string>(new Exception());
            Assert.AreEqual("bar", t.GetOrElse("bar"));
        }

        [Test]
        public void OrElse_on_Success_returns_Success() {
            var t = new Success<string>("foo");
            Assert.AreSame(t, t.OrElse(new Success<string>("bar")));
        }

        [Test]
        public void OrElse_on_Failure_returns_else() {
            var t = new Failure<string>(new Exception());
            var alt = new Success<string>("bar");
            Assert.AreSame(alt, t.OrElse(alt));
        }

        [Test]
        public void Recover_on_Success_does_not_call_closure() {
            var t = new Success<string>("foo");
            Exception e = null;
            Func<Exception, string> c = ex => {
                e = ex;
                return "bar";
            };
            Assert.AreEqual("foo", t.Recover(c).Value);
            Assert.IsNull(e);
        }

        [Test]
        public void Recover_on_Failure_does_call_closure() {
            var t = new Failure<string>(new Exception());
            Exception e = null;
            Func<Exception, string> c = ex => {
                e = ex;
                return "bar";
            };
            Assert.AreEqual("bar", t.Recover(c).Value);
            Assert.AreSame(t.Exception, e);
        }

        [Test]
        public void RecoverWith_on_Success_does_not_call_closure() {
            var t = new Success<string>("foo");
            Exception e = null;
            Func<Exception, Try<string>> c = ex => {
                e = ex;
                return new Success<string>("bar");
            };
            Assert.AreEqual("foo", t.RecoverWith(c).Value);
            Assert.IsNull(e);
        }

        [Test]
        public void RecoverWith_on_Failure_does_call_closure() {
            var t = new Failure<string>(new Exception());
            Exception e = null;
            Func<Exception, Try<string>> c = ex => {
                e = ex;
                return new Success<string>("bar");
            };
            Assert.AreEqual("bar", t.RecoverWith(c).Value);
            Assert.AreSame(t.Exception, e);
        }

        [Test]
        public void Transform_of_Success_calls_success_closure() {
            var t = new Success<int>(42);
            var successCalled = false;
            var failureCalled = false;
            Func<int, Try<string>> success = v => {
                successCalled = true;
                return new Success<string>(v.ToString());
            };
            Func<Exception, Try<string>> failure = e => {
                failureCalled = true;
                return new Success<string>(e.Message);
            };
            Assert.AreEqual("42", t.Transform(success, failure).Value);
            Assert.IsTrue(successCalled);
            Assert.IsFalse(failureCalled);
        }

        [Test]
        public void Transform_of_Failure_calls_failure_closure() {
            var t = new Failure<int>(new Exception("fail"));
            var successCalled = false;
            var failureCalled = false;
            Func<int, Try<string>> success = v => {
                successCalled = true;
                return new Success<string>(v.ToString());
            };
            Func<Exception, Try<string>> failure = e => {
                failureCalled = true;
                return new Success<string>(e.Message);
            };
            Assert.AreEqual("fail", t.Transform(success, failure).Value);
            Assert.IsFalse(successCalled);
            Assert.IsTrue(failureCalled);
        }

        [Test]
        public void Converting_Success_to_option_captures_value() {
            var t = new Success<string>("foo");
            var o = t.ToOption();
            Assert.IsTrue(o.IsDefined);
            Assert.AreEqual("foo", t.Value);
        }

        [Test]
        public void Converting_Failure_to_option_returns_none() {
            var t = new Failure<string>(new Exception());
            var o = t.ToOption();
            Assert.IsFalse(o.IsDefined);
            Assert.AreSame(Option<string>.None, o);
        }
    }
}