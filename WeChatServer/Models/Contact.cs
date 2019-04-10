
namespace WeChatServer.Models {
    /// <summary>
    /// 联系人
    /// </summary>
    public class Contact {
        public string ID { get; set; }
        public string UserID { get; set; }
        public string ContactID { get; set; }
        public string ContactName { get; set; }
        public string ContactAvatar { get; set; }
        public string LastMsg { get; set; }
    }
}
