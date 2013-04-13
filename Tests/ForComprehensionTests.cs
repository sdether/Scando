using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Scando;

namespace ScandoTests {
    [TestFixture]
    public class ForComprehensionTests {

        [Test]
        public void Can_select_option_from_try() {
            var t = new Success<Option<int>>(Option<int>.Some(42));
            var r = from a in t
                    from b in a
                    select b;
            Assert.AreEqual(42,r.Value);
        }

        [Test]
        public void Can_select_option_from_select_of_try_option_() {
            var r = from a in Api.CanFailOrBeNull("foo")
                    from b in a
                    select b;
            Assert.AreEqual("foo",r.Value);
        }


        [Test]
        public void Can_mix_and_match_Try_and_Option() {
            var r = from a in Api.CanFail("foo") 
                    from b in Api.CanFailOrBeNull(a)
                    from c in b
                    from d in Api.CanBeNull(c)
                    select d;
            Assert.AreEqual("foo",r.Value);
        }

        [Test]
        public void Can_use_where_in_try_comprehension() {
            var r = from a in Api.CanFail("foo") where a == "foo"
                    from b in Api.CanFail(a) where b == "foo"
                    select b;
            Assert.AreEqual("foo", r.Value);
        }

        [Test]
        public void Can_use_where_in_option_comprehension() {
            var r = from a in Api.CanBeNull("foo") where a == "foo"
                    from b in Api.CanBeNull(a) where b == "foo"
                    select b;
            Assert.AreEqual("foo", r.Value);
        }

        public static class Api {

            public static Try<string> CanFail(string input) {
                return new Success<string>(input);
            }

            public static Try<string> CanFail(Exception fail) {
                return new Failure<string>(fail);
            }

            public static Option<string> CanBeNull(string input) {
                return Option<string>.Some(input);
            }

            public static Try<Option<string>> CanFailOrBeNull(string input) {
                return new Success<Option<string>>(Option<string>.Some(input));
            }

            public static Try<Option<string>> CanFailOrBeNull(Exception fail) {
                return new Failure<Option<string>>(fail);
            }
        }
    }
}