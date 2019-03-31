using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using WeChatServer.Models;

namespace WeChatServer {
    [Authorize]
    public class ChatHub :Hub{
        #region 广播消息

        /// <summary>
        /// 广播消息
        /// </summary>
        /// <param name="user">fromUserID</param>
        /// <param name="msg">message</param>
        public void SendMessage(string user,string msg) {
            string name = Context.User.Identity.Name;
            name = Context.ConnectionId;
            Clients.All.SendAsync("ReceiveMessage", name, msg);
        }

        #endregion
    }

    /// <summary>
    /// 用于微信的ChatHub
    /// </summary>
    [Authorize]
    public class WeChatHub : Hub {
        /// <summary>
        /// 私人消息
        /// </summary>
        /// <param name="from">fromUserID</param>
        /// <param name="to">toUserID</param>
        /// <param name="msg">message</param>
        public void PersonalMessage(string from,string to,string msg) {
            Clients.User(to).SendAsync("ReceiveMessage",from, msg);
        }
    }

    
    public class WeChatUserIdProvider : IUserIdProvider {
        public string userID(IRequest request);

        public virtual string GetUserId(HubConnectionContext connection) {
            return connection.User?.FindFirst(ClaimTypes.Email)?.Value;
        }
    }
}
