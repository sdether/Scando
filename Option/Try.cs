using System;
using System.Collections;
using System.Collections.Generic;

namespace Option {

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