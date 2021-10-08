using long3x.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace long3x.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatabaseNotificationController : ControllerBase
    {
        private readonly ICustomObserversHelper customObserversHelper;

        public DatabaseNotificationController(ICustomObserversHelper customObserversHelper)
        {
            this.customObserversHelper = customObserversHelper;
        }

        [HttpGet("DatabaseUpdated")]
        public JsonResult DatabaseUpdated()
        {
            customObserversHelper.UpdateAll();
            return new JsonResult(new {Success = true});
        }
    }
}
