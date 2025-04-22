using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ApiGatewayWithTrotthleManagement.Controllers
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

        [HttpGet]
        public async Task<IActionResult> Get()
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
