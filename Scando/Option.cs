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
    public class Option<T> : IEnumerable<T> {

        public static readonly Option<T> None = new NoneImpl();

        private class NoneImpl : Option<T> {

            public override IEnumerator<T> GetEnumerator() {
                yield break;
            }

            public override T Get() {
                throw new InvalidOperationException("None does not have a value");
            }

            public override bool IsDefined { get { return false; } }

            public override int GetHashCode() {
                return 0;
            }
        }

        public static Option<T> Some(T value) {
            return object.Equals(value, null) ? None : new Option<T>(value);
        }

        private readonly T _value;

        private Option() { }

        private Option(T value) {
            _value = value;
        }

        public T Value { get { return Get(); } }
        public virtual bool IsDefined { get { return true; } }

        public Option<T> OrElse(Option<T> other) {
            return IsDefined ? this : other;
        }

        public virtual T Get() {
            return _value;
        }


        public T GetOrElse(T defaultValue) {
            return IsDefined ? _value : defaultValue;
        }

        public virtual IEnumerator<T> GetEnumerator() {
            yield return _value;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public override bool Equals(object other) {
            if(ReferenceEquals(null, other)) {
                return !IsDefined;
            }
            return other.Equals(_value);
        }

        public bool Equals(Option<T> other) {
            if(ReferenceEquals(null, other)) {
                return !IsDefined;
            }
            if(ReferenceEquals(this, other)) {
                return true;
            }
            return Equals(other._value, _value);
        }

        public override int GetHashCode() {
            return _value.GetHashCode();
        }
    }
}
