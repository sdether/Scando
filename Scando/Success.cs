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