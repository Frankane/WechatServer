using System;

namespace WeChatServer.Models {
    public class Essay {
        public string EssayID { get; set; }
        public string BookID { get; set; }
        public string UserID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }
        public Essay() {
            Time = DateTime.Now;
        }
    }
}
