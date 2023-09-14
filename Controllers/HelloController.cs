using System.Runtime.InteropServices.JavaScript;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace FirstWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelloController : ControllerBase
    {
        [HttpGet("refresh")]
        public async Task<IActionResult> RefreshTime(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                DateTime currentTime = DateTime.Now;
                string formattedTime = currentTime.ToString("yyyy-MM-dd HH:mm:ss");
                await Task.Delay(1000, cancellationToken); // Wait for 1 second
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Return the current time to the client
                    return Ok(formattedTime);
                }
            }

            return NoContent();
        }
        [HttpPost]
        public IActionResult Post(JObject payload)
        {

            return Ok(payload);
        }
    }
}
