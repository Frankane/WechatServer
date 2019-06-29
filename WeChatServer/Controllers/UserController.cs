using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using WeChatServer.Models;

namespace WeChatServer.Controllers {
  [Route("api/[controller]")]
  [ApiController]
  public class UserController : ControllerBase {

    SqlSugarClient db = SqlSugarHelper.ConnectMariaDB();

    #region 小程序用户登录获取openID

    /// <summary>
    /// 小程序用户登录获取openID
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    [Route("login")]
    public string Login(string code) {
      string url = "https://api.weixin.qq.com/sns/jscode2session";
      Dictionary<string, string> dic = new Dictionary<string, string>();
      dic.Add("appid", "wx7db2b6a240f66339");
      dic.Add("secret", "d5829e8f025b0c1b173b056885ac0187");
      dic.Add("js_code", code);
      dic.Add("grant_type", "authorization_code");
      string result = "";
      StringBuilder builder = new StringBuilder();
      builder.Append(url);
      if (dic.Count > 0) {
        builder.Append("?");
        int i = 0;
        foreach (var item in dic) {
          if (i > 0)
            builder.Append("&");
          builder.AppendFormat("{0}={1}", item.Key, item.Value);
          i++;
        }
      }

      HttpWebRequest req = (HttpWebRequest)WebRequest.Create(builder.ToString());
      //添加参数
      HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
      Stream stream = resp.GetResponseStream();
      try {
        //获取内容
        using (StreamReader reader = new StreamReader(stream)) {
          result = reader.ReadToEnd();
        }
      } finally {
        stream.Close();
      }
      //string[] st = result.Split('\"');
      //AddOrUpdate(st[7]);//将openid保存到User表
      return result;
    }

    #endregion

    #region 添加或者更新用户在线状态--[待优化]

    [Route("addorupdate")]
    public void AddOrUpdate(string openid, string connectionid, int online) {
      User user = new User { UserID = openid, ConnectionID = connectionid, Online = online };
      try {
        if (db.Queryable<User>().InSingle(openid).UserID == openid) {
          db.Updateable(user).ExecuteCommand();
        }
      } catch (System.Exception) {
        db.Insertable(user).ExecuteCommand();
      }
    }

    #endregion

    #region 获取个人全部图书 参数:ownerid(userid/openid)

    /// <summary>
    /// 根据ownerID获取用户所有图书
    /// </summary>
    /// <param name="ownerid">用户ID(openid)</param>
    /// <returns></returns>
    [Route("getmybooks")]
    public ActionResult<IEnumerable<Book>> GetMyBooks(string ownerid) {
      return db.Queryable<Book>().Where(it => it.OwnerID == ownerid).ToList();
    }

    #endregion

    #region 获取联系人 //可能没用！！！

    [Route("getcontacts")]
    public ActionResult<IEnumerable<Contact>> GetContacts(string userid) {
      try {
        return db.Queryable<Contact>().Where(user => user.UserID == userid).ToList();
      } catch { }
      return null;
    }

    #endregion

    #region 获取未读消息

    // 获取用户未读消息
    [Route("getmessages")]
    public IEnumerable<IGrouping<string, Message>> GetMessages(string userid) {
      try {

        List<Message> messageList = db.Queryable<Message>().Where(user => user.ToID == userid).ToList();
        messageList.Add(db.Queryable<Message>().InSingle("1"));
        db.Deleteable<Message>().Where(user => user.ToID == userid).ExecuteCommand();
        IEnumerable<IGrouping<string, Message>> group =
            from message in messageList group message by message.FromID into g select g;
        return group;
        //foreach (var one in group) {
        //    if (db.Queryable<Contact>().Where(user=>user.ContactID==one.Key)!=null) {

        //    }
        //    foreach (var o in one) {
        //        string i = o.ID;
        //    }
        //}
      } catch {
        List<Message> messageList = null;
        messageList.Add(db.Queryable<Message>().InSingle("1"));
        IEnumerable<IGrouping<string, Message>> group =
            from message in messageList group message by message.FromID into g select g;
        return group;
      }
    }

    #endregion

    #region 获取用户消息-头像、昵称

    [Route("getuserinfo")]
    public ActionResult<Book> getUserInfo(string userid) {
      try {
        return db.Queryable<Book>().First(user => user.OwnerID == userid);
      } catch { return null; }

    }

    #endregion


    public void newContact(string userid, Message msg) {
      Dictionary<string, Message> contact = new Dictionary<string, Message>();
      contact.Add(userid, msg);
    }

    #region 添加联系人

    [Route("addcontact"), HttpPost]
    public void AddContact() {
      Contact contact = new Contact {
        ID = Guid.NewGuid().ToString("N"),
        UserID = Request.Form["userid"],
        ContactID = Request.Form["contactid"],
        ContactAvatar = Request.Form["contactAvatar"],
        ContactName = Request.Form["contactName"],
        LastMsg = Request.Form["lastMsg"]
      };
      db.Insertable(contact);
    }

    #endregion

  }
}
