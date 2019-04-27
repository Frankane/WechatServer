using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System.Collections.Generic;
using WeChatServer.Models;

namespace WeChatServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AphorismController : ControllerBase {
        SqlSugarClient db = SqlSugarHelper.ConnectMariaDB();

        /// <summary>
        /// 添加格言
        /// </summary>
        /// <param name="quote"></param>
        /// <param name="author"></param>
        /// <returns></returns>
        [Route("addAphorism")]
        public ActionResult AddQuote(string aphorism, string userid, string author = "佚名") {
            Aphorism newAphorism = new Aphorism { aphorism = aphorism, UserID = userid, Author = author };
            db.Insertable(newAphorism).ExecuteCommand();
            return Ok("添加成功");
        }

        /// <summary>
        /// 随机获取一个格言
        /// </summary>
        /// <returns></returns>
        [Route("getaphorism")]
        public ActionResult<IEnumerable<Aphorism>> GetQuote() {
            return db.Queryable<Aphorism>().OrderBy("rand() limit 1").ToList();
        }

        [HttpGet, Route("allAphorisms")]
        public ActionResult<IEnumerable<Aphorism>> AllAphorisms(string userid = "") {
            if (userid == "") {
                return db.Queryable<Aphorism>().OrderBy(it => it.Time, OrderByType.Desc).ToList();
            }
            else {
                return db.Queryable<Aphorism>().Where(it => it.UserID == userid).OrderBy(it => it.Time, OrderByType.Desc).ToList();
            }
        }
    }
}