using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeChatServer.Models {
    public class Aphorism {
        public string ID { get; set; }
        public string aphorism { get; set; }
        public string Author { get; set; }
        public string UserID { get; set; }
        public DateTime Time { get; set; }
        public Aphorism() {
            aphorism = "佚名";
            ID = Guid.NewGuid().ToString("N");
            Time = DateTime.Now;
        }
    }
}
