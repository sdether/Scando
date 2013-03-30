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
using System.Collections.Generic;

namespace Scando {
    public class Success<T> : Try<T> {
        private readonly T _value;

        public Success(T value) {
            _value = value;
        }

        public override T Value { get { return Get(); } }
        public override bool IsSuccess { get { return true; } }
        public override bool IsFailure { get { return false; } }

        public override T Get() {
            return _value;
        }

        public override T GetOrElse(T defaultValue) {
            return _value;
        }

        public override Try<T> Recover(Func<Exception, T> recoverClosure) {
            return this;
        }

        public override Try<T> RecoverWith(Func<Exception, Try<T>> recoverClosure) {
            return this;
        }

        public override Try<V> Transform<V>(Func<T, Try<V>> success, Func<Exception, Try<V>> failure) {
            return success(_value);
        }

        public override Option<T> ToOption() {
            return Option<T>.Some(_value);
        }

        public override IEnumerator<T> GetEnumerator() {
            yield return _value;
        }
    }
}