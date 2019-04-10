using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using WeChatServer.Models;
using SqlSugar;

namespace WeChatServer.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase {

        #region 变量和配置

        SqlSugarClient db = SqlSugarHelper.ConnectMariaDB();
        private IHostingEnvironment hostingEnvironment;
        public BookController(IHostingEnvironment env) {
            hostingEnvironment = env;
        }

        #endregion

        #region 主页

        /// <summary>
        /// 主页
        /// </summary>
        /// <returns></returns>
        // GET api/books
        [HttpGet]
        public ActionResult<IEnumerable<string>> Home() => new string[] { "欢迎访问!这是微信小程序后台首页,现在是:" + DateTime.Now.ToLongDateString() };

        #endregion

        #region 随机获取三本书
        [Route("getthreebooks")]
        public ActionResult<IEnumerable<Book>> GetThreeBooks() {
            return db.Queryable<Book>().OrderBy("rand() limit 3").ToList();
        }

        #endregion

        #region 根据bookID获取书籍信息
        [Route("bookinfo")]
        public ActionResult<Book> getBookByID(string bookID) {
            return db.Queryable<Book>().InSingle(bookID);
        }
        #endregion

        #region 书籍搜索

        /// <summary>
        /// 书籍搜索
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        //[Route("api/[controller]/search")]
        //[HttpGet("{keyword}")]
        [Route("search")]
        public ActionResult<IEnumerable<Book>> Search(string keyword) {
            List<Book> result1 = db.Queryable<Book>().Where(s => s.Name.Contains(keyword)).ToList();
            List<Book> result2 = db.Queryable<Book>().Where(s => s.Author.Contains(keyword)).ToList();
            List<Book> result = result1.Union(result2, new BookId()).ToList();

            return result;
        }

        #endregion

        #region 查看书籍详情

        /// <summary>
        /// 查看一本书的详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //GET api/books/5
        //[Route("api/[controller]/getbook")]
        //[HttpGet("{bookid}")]
        [Route("getabook")]
        public ActionResult<Essay> GetBook(string bookid) {
            Console.WriteLine("执行查看操作-------------");

            return new Essay();
        }

        #endregion

        #region 添加书籍

        /// <summary>
        /// 添加一本书
        /// </summary>
        /// <param name="name"></param>
        /// <param name="author"></param>
        /// <param name="owner"></param>
        /// <param name="introduce"></param>
        [HttpPost, Route("addbook")]
        public ActionResult AddBook() {
            
            IFormFile file = Request.Form.Files["bookcover"];
            // 文件大小
            //long size = 0;
            // 原文件名（包括路径）
            var filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;
            // 扩展名
            var extName = filename.Substring(filename.LastIndexOf('.')).Replace("\"", "");
            // 新文件名
            string shortfilename = $"{Guid.NewGuid().ToString("N")}{extName}";
            // 新文件名（包括路径）
            filename = hostingEnvironment.WebRootPath + @"/Images/BookCovers/" + shortfilename;
            // 设置文件大小
            //size += file.Length;
            // 创建新文件
            using (FileStream fs = System.IO.File.Create(filename)) {
                // 复制文件
                file.CopyTo(fs);
                // 清空缓冲区数据
                fs.Flush();
            }
            
            //将书籍封面地址和信息保存到数据库
            Book book = new Book {
                BookID = Guid.NewGuid().ToString("N"),
                Name = Request.Form["bookName"],
                Author = Request.Form["bookAuthor"],
                OwnerID = Request.Form["ownerid"],
                Introduce = Request.Form["bookIntro"],
                UploadTime = DateTime.Now,
                BookCover = "filename"
            };
            db.Insertable(book).ExecuteCommand();
            return Ok("添加成功");
        }

        #endregion

        #region 添加文章

        [Route("addessay")]
        public void addEssay(string title,string content,string userid,string avatar,string nickname) {
            Essay newEssay = new Essay {
                EssayID = Guid.NewGuid().ToString("N"),
                Title = title,
                UserID = userid,
                AvatarUrl = avatar,
                Content = content,
                UserNickName = nickname,
                Time = DateTime.Now
            };
            db.Insertable(newEssay).ExecuteCommand();
        }

        #endregion

        #region 获取文章

        [Route("getessay")]
        public ActionResult<IEnumerable<Essay>> getEssays(string essayid="") {
            if (essayid!="") {
                return new List<Essay> { db.Queryable<Essay>().InSingle(essayid) };
            }
            return db.Queryable<Essay>().ToList();
        }

        #endregion

        #region 上传图书封面

        /// <summary>
        /// 接收小程序端上传的图书封面
        /// </summary>
        [HttpPost]
        [Route("uploadcover")]
        public ActionResult UploadBookCover() {
            IFormFile file = Request.Form.Files["bookcover"];

            // 文件大小
            //long size = 0;
            // 原文件名（包括路径）
            var filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;
            // 扩展名
            var extName = filename.Substring(filename.LastIndexOf('.')).Replace("\"", "");
            // 新文件名
            string shortfilename = $"{Guid.NewGuid().ToString("N")}{extName}";
            // 新文件名（包括路径）
            filename = hostingEnvironment.WebRootPath + @"\Images\BookCovers\" + shortfilename;
            // 设置文件大小
            //size += file.Length;
            // 创建新文件
            using (FileStream fs = System.IO.File.Create(filename)) {
                // 复制文件
                file.CopyTo(fs);
                // 清空缓冲区数据
                fs.Flush();
            }
            // 将图书封面路径保存到数据库
            return Ok("封面上传成功");
        }

        #endregion

        #region 下载图书封面
        /// <summary>
        /// 下载图书封面
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getbookcover")]
        public ActionResult DownLoadBookCover(string file) {
            var addrUrl = hostingEnvironment.WebRootPath + @"\Images\BookCovers\" + file;
            var stream = System.IO.File.OpenRead(addrUrl);
            string fileExt = file.Substring(file.LastIndexOf('.')).Replace("\"", "");
            //获取文件的ContentType
            var provider = new FileExtensionContentTypeProvider();
            var memi = provider.Mappings[fileExt];
            return File(stream, memi, Path.GetFileName(addrUrl));
        }
        #endregion

        #region 修改书籍信息

        /// <summary>
        /// 修改一本书
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        // PUT api/books/5
        //[HttpPut("{id}")]
        [Route("change")]
        public void ChangeBook(string bookid, [FromBody] string value) {
            Console.WriteLine("执行修改操作-------------");
        }

        #endregion

        #region 删除书籍

        /// <summary>
        /// 删除一本书
        /// </summary>
        /// <param name="bookid"></param>
        // DELETE api/books/5
        //[HttpDelete("{id}")]
        [Route("delete")]
        public void DeleteBook(string bookid) {
            Console.WriteLine("执行删除操作-------------");

        }

        #endregion
    }
}
