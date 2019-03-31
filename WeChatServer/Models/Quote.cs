using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeChatServer.Models {
    public class Quote {
        public int id { get; set; }
        public string quote { get; set; }
        public string author { get; set; }
        public Quote() {
            author = "佚名";
        }
    }
}
