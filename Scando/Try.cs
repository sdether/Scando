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
using System.Collections;
using System.Collections.Generic;

namespace Scando {

    public class Try<T> : IEnumerable<T> {
        public static Try<T> Eval(Func<T> closure) {
            try {
                return new Success<T>(closure());
            } catch(Exception e) {
                return new Failure<T>(e);
            }
        }

        protected Try() { }

        public virtual T Value { get { return Get(); } }
        public virtual Exception Exception { get { throw new InvalidOperationException("Try did not succeed"); } }
        public virtual bool IsSuccess { get { return false; } }
        public virtual bool IsFailure { get { return true; } }

        public virtual T Get() {
            throw new InvalidOperationException("Try did not succeed");
        }

        public Try<T> OrElse(Try<T> other) {
            return IsSuccess ? this : other;
        }

        public virtual T GetOrElse(T defaultValue) {
            return defaultValue;
        }

        public virtual Try<T> Recover(Func<Exception,T> recoverClosure) {
            throw new NotImplementedException();
        }

        public virtual Try<T> RecoverWith(Func<Exception, Try<T>> recoverClosure) {
            throw new NotImplementedException();
        }

        public virtual Try<V> Transform<V>(Func<T, Try<V>> success, Func<Exception, Try<V>> failure) {
            throw new NotImplementedException();
        }

        public virtual IEnumerator<T> GetEnumerator() {
            yield break;
        }

        public virtual Option<T> ToOption() {
            return Option<T>.None;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}