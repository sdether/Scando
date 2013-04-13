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
using System.Linq;

namespace Scando {
    public static class TryLinqExtensions {

        public static Try<T> Where<T>(this Try<T> trial, Func<T, bool> predicate) {
            if(trial.IsFailure) {
                return trial;
            }
            return predicate(trial.Value) ? trial : new Failure<T>(new NoSuchElementException());
        }

        public static Try<V> Select<T, V>(this Try<T> trial, Func<T, V> selector) {
            return trial.IsSuccess ? (Try<V>)new Success<V>(selector(trial.Value)) : new Failure<V>(trial.Exception);
        }

        public static Try<TResult> SelectMany<T, TResult>(this Try<T> trial, Func<T, Try<TResult>> selector) {
            return SelectMany(trial, selector, (x, y) => y);
        }

        public static Try<TResult> SelectMany<T, TCollection, TResult>(this Try<T> trial, Func<T, Try<TCollection>> collectionSelector, Func<T, TCollection, TResult> resultSelector) {
            if(trial.IsFailure) {
                return new Failure<TResult>(trial.Exception);
            }
            var c = collectionSelector(trial.Value);
            if(c.IsFailure) {
                return new Failure<TResult>(c.Exception);
            }
            var r = resultSelector(trial.Value, c.Value);
            return new Success<TResult>(r);
        }

        public static Option<TResult> SelectMany<T, TCollection, TResult>(this Try<T> trial, Func<T, Option<TCollection>> collectionSelector, Func<T, TCollection, TResult> resultSelector) {
            if(trial.IsFailure) {
                return Option<TResult>.None;
            }
            var c = collectionSelector(trial.Value);
            if(!c.IsDefined) {
                return Option<TResult>.None;
            }
            var r = resultSelector(trial.Value, c.Value);
            return Option<TResult>.Some(r);
        }

        public static Try<T> ToTry<T>(this IEnumerable<T> enumerable) {
            return enumerable.Any() ? (Try<T>)new Success<T>(enumerable.First()) : new Failure<T>(new NoSuchElementException());
        }

    }
}