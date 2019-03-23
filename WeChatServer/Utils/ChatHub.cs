using Microsoft.AspNetCore.SignalR;

namespace WeChatServer {
    public class ChatHub :Hub{

        public void SendMessage(string user,string msg) {
            Clients.All.SendAsync("ReceiveMessage",user,msg);
        }
    }
}
