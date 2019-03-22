using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeChatServer.Models {
    public class BookId : IEqualityComparer<Book> {
        public bool Equals(Book x, Book y) {
            return x.BookID == y.BookID;
        }

        public int GetHashCode(Book obj) {
            if (obj == null) {
                return 0;
            }
            else {
                return obj.ToString().GetHashCode();
            }
        }
    }
}
