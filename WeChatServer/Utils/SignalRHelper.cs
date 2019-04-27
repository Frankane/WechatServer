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
        /// 断开连接时更新用户的在线状态
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override Task OnDisconnectedAsync(Exception exception) {
            try {
                db.Updateable<User>().Where(user => user.ConnectionID == Context.ConnectionId)
                    .ReSetValue(it => it.Online == 0).ExecuteCommand();
            }
            catch  {}
            return base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// 私人消息
        /// </summary>
        /// <param name="from">fromUserID</param>
        /// <param name="to">toUserID</param>
        /// <param name="msg">message</param>
        public void PersonalMessage(string contactid,string fromid,string toid,string nickName,string fromAvatar,string msg) {
            try {
                User user = db.Queryable<User>().InSingle(toid);
                // 在线时发送消息                    
                if (user.Online==1) {
                    Clients.Client(user.ConnectionID).SendAsync("ReceiveMessage", contactid,fromid, toid,nickName,fromAvatar,msg);
                }
                // 离线时保存消息
                else {
                    Message message = new Message {
                        ID = Guid.NewGuid().ToString("N"),FromID = fromid,ToID = toid,
                        Content = msg, FromName = nickName, FromAvatar = fromAvatar,ContactID=contactid
                    };
                    db.Insertable(message).ExecuteCommand();
                }
                #region 联系人存不存在于用户列表[不用在这里设置]
                //// 如果联系人存在
                //if(db.Queryable<Contact>().Where(it=>(it.ContactID==fromid && it.UserID==toid))!=null){

                //}
                //// 如果联系人不存在
                //else{
                //    // 添加联系人
                //    Contact c = new Contact(){
                //        UserID=toid,ContactID=fromid,ContactAvatar=fromAvatar,LastMsg=msg,ContactName=nickName
                //    };db.Insertable(c).ExecuteCommand();
                //    // 在线时发送消息
                //    if (user.Online==1) {
                //        Clients.Client(user.ConnectionID).SendAsync("ReceiveMessage", fromid,fromAvatar, msg);
                //    }
                //    // 离线时保存消息
                //    else {
                //        Message message = new Message {
                //            ID = Guid.NewGuid().ToString("N"),                           
                //            FromID = fromid, ToID = toid, Content = msg, FromName = nickName, FromAvatar = fromAvatar

                //        };
                //        db.Insertable(message).ExecuteCommand();
                //    }
                //}
                #endregion
            }
            catch { }
        }

    }
}
