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
    public static class LinqExtensions {

        public static Option<V> Select<T, V>(this Option<T> option, Func<T, Option<V>> selector) {
            return option.IsDefined ? selector(option.Value) : Option<V>.None;
        }

        public static Option<T> ToOption<T>(this IEnumerable<T> enumerable) {
            return enumerable.Any() ? Option<T>.Some(enumerable.First()) : Option<T>.None;
        }

        public static Try<V> Select<T, V>(this Try<T> option, Func<T, Try<V>> selector) {
            return option.IsSuccess ? selector(option.Value) : new Failure<V>(option.Exception);
        }

        public static Try<T> Where<T>(this Try<T> source, Func<T, bool> predicate) {
            if(source.IsFailure) {
                return source;
            }
            return predicate(source.Value) ? source : new Failure<T>(new NoSuchElementException());
        }
    }

    public class NoSuchElementException : Exception {}
}