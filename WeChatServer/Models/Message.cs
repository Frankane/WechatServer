using System;

namespace WeChatServer.Models {
    public class Message {
        public string ID { get; set; }
        public string FromID { get; set; }
        public string ToID { get; set; }
        public string FromName { get; set; }         
        public string FromAvatar { get; set; }         
        public DateTime Time { get; set; }
        public string Content { get; set; }
        public string ContactID { get; set; }

        public Message() {
            Time = DateTime.Now;
        }
    }
}
