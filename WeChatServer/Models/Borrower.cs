using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeChatServer.Models {
    public class Borrower {
        public int AppID{get;set;}
        public string Name{get;set;}
        public string BookName{get;set;}
        public string BookID{get;set;}
        public DateTime BorrowTime{get;set;}
    }
}
