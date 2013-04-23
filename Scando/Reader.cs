using System;

namespace Scando {
    public class Reader<TFrom,TTo> {
        private readonly Func<TFrom, TTo> _wrappedF;

        public Reader(Func<TFrom,TTo> wrappedF) {
            _wrappedF = wrappedF;
        }

        public TTo Apply(TFrom c) {
            return _wrappedF(c);
        }

        public static Reader<TFrom,TToB> Select<TToB>(Func<TTo, TToB> selector) {
            return null;
        }

        //public static Try<TResult> SelectMany<T, TResult>(this Try<T> trial, Func<T, Try<TResult>> selector) {
        //    return SelectMany(trial, selector, (x, y) => y);
        //}

        public static Reader<TFrom, TToB> SelectMany<T, TCollection, TToB>(Func<T, Try<TCollection>> collectionSelector, Func<T, TCollection, TToB> resultSelector) {
            return null;
        }
    }
}