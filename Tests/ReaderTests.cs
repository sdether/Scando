using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Scando;

namespace ScandoTests {
    
    [TestFixture]
    public class ReaderTests {

        public class User {}

        public class Post {}

        public interface IUserConnection {
            Option<User> ReadUser(long id);
        }

        public interface IPostConnection {
            IEnumerable<Post> ReadPosts(User user);
        }


        public Reader<Tuple<IUserConnection, IPostConnection>, IEnumerable<Post>> UserPosts(long userId) {
            return new Reader<Tuple<IUserConnection, IPostConnection>, IEnumerable<Post>>(conn =>
                conn.Item1.ReadUser(userId).Select(u => conn.Item2.ReadPosts(u)).GetOrElse(new Post[0])
            );
        }

        public Reader<Tuple<IUserConnection, IPostConnection>, IEnumerable<Post>> UserPosts2(long userId) {
            return new Reader<Tuple<IUserConnection, IPostConnection>, IEnumerable<Post>>(conn =>
                from u in conn.Item1.ReadUser(userId)
                from p in conn.Item2.ReadPosts(u)
                select p
            );
        }

        public IEnumerable<Post> FetchPosts() {
            IUserConnection userConn = null;
            IPostConnection postConn = null;
            var posts = UserPosts(123);
            return posts.Apply(new Tuple<IUserConnection, IPostConnection>(userConn, postConn));
        }
    }
}
