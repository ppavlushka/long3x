using long3x.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace long3x.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITraderInfoRepository traderInfoRepository;

        public HomeController(ITraderInfoRepository traderInfoRepository)
        {
            this.traderInfoRepository = traderInfoRepository;
        }

        public IActionResult Index()
        {
            var data = traderInfoRepository.GetAllTraderInfo();
            return View(data);
        }
    }
}
