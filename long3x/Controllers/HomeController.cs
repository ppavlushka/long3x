using System.Linq;
using long3x.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace long3x.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISignalRepository signalRepository;

        public HomeController(ISignalRepository signalRepository)
        {
            this.signalRepository = signalRepository;
        }

        public IActionResult Index()
        {
            var data = signalRepository.GetAllSignals().Reverse();
            return View(data);
        }
    }
}
