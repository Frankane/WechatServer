using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
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
            }
            finally {
                stream.Close();
            }
            //string[] st = result.Split('\"');
            //AddOrUpdate(st[7]);//将openid保存到User表
            return result;
        }

        #endregion

        #region 添加或者更新--[待优化]

        [Route("addorupdate")]
        public void AddOrUpdate(string openid,string connectionid,int online) {
            User user = new User { UserID = openid, ConnectionID=connectionid ,Online=online};
            
            try {
                if (db.Queryable<User>().InSingle(openid).UserID == openid) {
                    db.Updateable(user).ExecuteCommand();
                }
            }
            catch (System.Exception) {
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

        #region 获取联系人

        [Route("getcontacts")]
        public ActionResult<IEnumerable<Contact>> GetContacts(string userid) {
            try {
                return db.Queryable<Contact>().Where(user => user.UserID == userid).ToList();
            } catch {}
            return null;
        }

        #endregion

        #region 添加联系人

        [Route("addcontact"),HttpPost]
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
