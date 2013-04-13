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
    public static class OptionLinqExtensions {

        public static Option<T> Where<T>(this Option<T> option, Func<T, bool> predicate) {
            if(!option.IsDefined) {
                return option;
            }
            return predicate(option.Value) ? option : Option<T>.None;
        }

        public static Option<TResult> Select<TSource,TResult>(this Option<TSource> option, Func<TSource, TResult> selector) {
            return option.IsDefined ? Option<TResult>.Some(selector(option.Value)) : Option<TResult>.None;
        }

        public static Option<TResult> SelectMany<TSource,TResult>(this Option<TSource> option, Func<TSource, Option<TResult>> selector) {
            return SelectMany(option, selector, (x, y) => y);
        }

        public static Option<TResult> SelectMany<TSource, TCollection, TResult>(this Option<TSource> option, Func<TSource, Option<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector) {
            if(!option.IsDefined) {
                return Option<TResult>.None;
            }
            var c = collectionSelector(option.Value);
            if(!c.IsDefined) {
                return Option<TResult>.None;
            }
            var r = resultSelector(option.Value, c.Value);
            return Option<TResult>.Some(r);
        }

        public static Option<T> ToOption<T>(this IEnumerable<T> enumerable) {
            return enumerable.Any() ? Option<T>.Some(enumerable.First()) : Option<T>.None;
        }
    }
}