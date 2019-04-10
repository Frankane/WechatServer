using System;

namespace WeChatServer.Models {
    public class Message {
        public string ID { get; set; }
        public string FromID { get; set; }
        public string ToID { get; set; }
        public DateTime Time { get; set; }
        public string Content { get; set; }

        public Message() {
            Time = DateTime.Now;
        }
    }
}
