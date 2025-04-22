using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace apigw_monitor.Controllers
{
    [ApiController]
    [Route("proxy/[controller]")]
    public class ButchController : Controller
    {
        private readonly BackendThrottler _throttler;

        public ButchController(BackendThrottler throttler)
        {
            _throttler = throttler;
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            var result = await _throttler.ForwardRequest(HttpContext);

            if (result is ContentResult contentResult)
            {
                return contentResult;
            }
            else if (result is ObjectResult objectResult)
            {
                return objectResult;
            }
            else if (result is StatusCodeResult statusCodeResult)
            {
                return statusCodeResult;
            }
            else
            {
                return StatusCode(500, "Unexpected result");
            }
        }
    }
}
