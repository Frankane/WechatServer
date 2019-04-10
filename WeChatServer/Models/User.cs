using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeChatServer.Models {
    /// <summary>
    ///用户类，用于用户间的聊天
    /// </summary>
    public class User {
        public string UserID { get; set; }
        public string ConnectionID { get; set; }
        public int Online { get; set; }
    }
}
