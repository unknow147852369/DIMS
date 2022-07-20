using Microsoft.AspNetCore.Mvc;

namespace DIMSApis.Controllers
{
    public class PhotoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
