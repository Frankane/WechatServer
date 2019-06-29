using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WeChatServer.Models;

namespace WeChatServer.Controllers {
  public class HomeController : Controller {
    public IActionResult Index() {
      return View();
    }

    public IActionResult Privacy() {
      return View();
    }
    public IActionResult Servey() {
      return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}
