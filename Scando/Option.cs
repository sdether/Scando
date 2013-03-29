using System;
using System.Collections;
using System.Collections.Generic;

namespace Scando {
    internal static class NoneBase {
        public static readonly int NoneHashcode = new object().GetHashCode();
    }

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
                return NoneBase.NoneHashcode;
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
