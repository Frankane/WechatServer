using Microsoft.AspNetCore.Mvc;
using WeChatServer.Models;

namespace WeChatServer.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase  {

        /// <summary>
        /// 广播消息
        /// </summary>
        /// <param name="fromUser"></param>
        /// <param name="toUser"></param>
        /// <param name="msg"></param>
        [Route("sendAll")]
        public void BroadCast(string toUser, string msg) {
            ChatHub chathub = new ChatHub();
            chathub.SendMessage(toUser, msg);

        }

        /// <summary>
        /// 发消息
        /// </summary>
        /// <param name="from"></param>
        /// <param name="toUser"></param>
        /// <param name="msg"></param>
        //[Route("send")]
        //public void PersonalChat(User fromUser, User toUser, string msg) {
        //    ChatHub chathub = new ChatHub();
        //    chathub.SendMessage(toUser.ID, msg);
        //    chathub.PersonalMessage(toUser, msg);

        //}

        //[HubMethodName("SendMessageToUser")]
        //public Task DirectMessage(string user, string message) {
        //    return Clients.User(user).SendAsync("ReceiveMessage", message);
        //}

    }
}