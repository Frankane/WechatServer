using System;
using System.Collections.Generic;

namespace WeChatServer.Models {
    /// <summary>
    /// 书
    /// </summary>
    public class Book {
        public string BookID { get; set; }//书的唯一ID
        public string Name { get; set; }//书名
        public string Author { get; set; }//作者
        public string OwnerID { get; set; }//拥有者ID
        public string Introduce { get; set; }//书籍简介
        public DateTime UploadTime { get; set; }//上传时间
        public string BookCover { get; set; }//封面存储路径
        public string Condition { get; set; }//书籍状态:可借/不可借
        public string IconCondition { get; set;}
        public Book() {
            Condition = "可借";
            IconCondition = "success";
        }
    }

    /// <summary>
    /// 根据BookID取消重复项——(用于Union方法的一个参数)
    /// </summary>
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
