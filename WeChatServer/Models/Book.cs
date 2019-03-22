using System;

namespace WeChatServer.Models {
    public class Book {
        public string BookID { get; set; }//书的唯一ID
        public string Name { get; set; }//书名
        public string Author { get; set; }//作者
        public string Owner { get; set; }//拥有者
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
}
