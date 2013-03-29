using System;

namespace Option {
    public static class LinqExtensions {

        public static Option<V> Select<T, V>(this Option<T> option, Func<T, Option<V>> selector) {
            if(option.IsDefined) {
                return selector(option.Value);
            }
            return Option<V>.None;
        }
        public static Try<V> Select<T, V>(this Try<T> option, Func<T, Try<V>> selector) {
            return option.IsSuccess 
                ? selector(option.Value) 
                : new Failure<V>(option.Exception);
        }
    }
}