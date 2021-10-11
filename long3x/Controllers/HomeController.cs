using long3x.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace long3x.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISignalService signalService;

        public HomeController(ISignalService signalService)
        {
            this.signalService = signalService;
        }

        public IActionResult Index()
        {
            var viewModel = signalService.GetSignalViewModels();
            return View(viewModel);
        }
    }
}
