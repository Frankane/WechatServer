using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;


namespace WeChatServer.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase  {
        
        [HttpPost]
        [Route("personal")]
        public void PersonalChat(string from, string to,string msg) {
            ChatHub chathub = new ChatHub();
            chathub.SendMessage(to, msg);
        }

    }
}