using Microsoft.AspNetCore.SignalR;
using SqlSugar;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using WeChatServer.Models;

namespace WeChatServer {
    public class ChatHub :Hub{
        #region 广播消息

        /// <summary>
        /// 广播消息
        /// </summary>
        /// <param name="user">fromUserID</param>
        /// <param name="msg">message</param>
        public void SendMessage(string user,string msg) {
            //string name = Context.User.Identity.Name;
            //string connectionid = Context.ConnectionId;
            Clients.All.SendAsync("ReceiveMessage", user, msg);
        }

        #endregion
    }

    /// <summary>
    /// 用于微信的ChatHub
    /// </summary>
    public class WeChatHub : Hub {
        SqlSugarClient db = SqlSugarHelper.ConnectMariaDB();

        public override Task OnConnectedAsync() {
            //连接后将它的connectionid发给他
            Clients.Client(Context.ConnectionId).SendAsync("ReceiveConnectionID", Context.ConnectionId);
            
            return base.OnConnectedAsync();
        }

       
        /// <summary>
        /// 私人消息
        /// </summary>
        /// <param name="from">fromUserID</param>
        /// <param name="to">toUserID</param>
        /// <param name="msg">message</param>
        public void PersonalMessage(string from,string to,string fromAvatar,string msg) {
            try {
                User user = db.Queryable<User>().InSingle(to);
                if (user.Online==1) {
                    Clients.Client(user.ConnectionID).SendAsync("ReceiveMessage", from,fromAvatar, msg);
                }
                else {
                    Message message = new Message {
                        ID = Guid.NewGuid().ToString("N"),
                        FromID = from, ToID = to, Content = msg
                    };
                    db.Insertable(message).ExecuteCommand();
                }
            }
            catch {
                
            }

        }
        
    }

    
    public class WeChatUserIdProvider : IUserIdProvider {
        //public string UserID(IRequest request);

        public virtual string GetUserId(HubConnectionContext connection) {
            return connection.User?.FindFirst(ClaimTypes.Email)?.Value;
        }
    }
}
