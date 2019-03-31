using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System.Collections.Generic;
using WeChatServer.Models;

namespace WeChatServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuoteController : ControllerBase
    {
        SqlSugarClient db = SqlSugarHelper.ConnectMariaDB();

        /// <summary>
        /// 添加格言
        /// </summary>
        /// <param name="quote"></param>
        /// <param name="author"></param>
        /// <returns></returns>
        [Route("addquote")]
        public ActionResult AddQuote(string quote,string author) {
            Quote newQuote = new Quote { quote = quote, author = author };
            db.Insertable<Quote>(newQuote);
            return Ok("添加成功");
        }

        /// <summary>
        /// 随机获取一个格言
        /// </summary>
        /// <returns></returns>
        [Route("getquote")]
        public ActionResult<IEnumerable<Quote>> GetQuote() {
            return db.Queryable<Quote>().OrderBy("rand() limit 1").ToList();
        }
    }
}