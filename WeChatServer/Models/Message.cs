using System;

namespace WeChatServer.Models {
    public class Message {
        public string fromID { get; set; }
        public string toID { get; set; }
        public string fromAvatar { get; set; }
        public string toAvatar { get; set; }
        public DateTime msgTime { get; set; }
        public string message { get; set; }

        public Message() {
            msgTime = DateTime.Now;
        }
    }
}
