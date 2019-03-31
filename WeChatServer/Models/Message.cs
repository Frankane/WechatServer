using System;

namespace WeChatServer.Models {
    public class Message {
        public int ID { get; set; }
        public string FromID { get; set; }
        public string ToID { get; set; }
        public DateTime MsgTime { get; set; }
        public string Content { get; set; }

        public Message() {
            MsgTime = DateTime.Now;
        }
    }
}
