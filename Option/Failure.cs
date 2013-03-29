using System;

namespace Option {
    public class Failure<T> : Try<T> {
        private readonly Exception _exception;

        public Failure(Exception exception) {
            _exception = exception;
        }

        public override Exception Exception { get { return _exception; } }

        public override Try<T> Recover(Func<Exception, T> recoverClosure) {
            return Eval(() => recoverClosure(_exception));
        }

        public override Try<T> RecoverWith(Func<Exception, Try<T>> recoverClosure) {
            return recoverClosure(_exception);
        }

        public override Try<V> Transform<V>(Func<T, Try<V>> success, Func<Exception, Try<V>> failure) {
            return failure(_exception);
        }
    }
}