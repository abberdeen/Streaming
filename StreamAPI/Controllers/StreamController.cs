using Microsoft.AspNetCore.Mvc;

namespace StreamAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StreamController : ControllerBase
    { 

        private readonly ILogger<StreamController> _logger;

        public StreamController(ILogger<StreamController> logger)
        {
            _logger = logger;
        }


        [HttpGet("")]
        public async Task GetStream()
        {
            byte[] buffer = new byte[1024 * 16]; // 16KB buffer

            Response.ContentType = "application/json";

            // Important 
            Response.Headers["Content-Encoding"] = "identity";
            Response.Headers["Transfer-Encoding"] = "identity";

            using (StreamWriter writer = new StreamWriter(Response.Body))
            {
                // In this example, we write 1000 lines of text,
                // with each line being 100 characters long.
                for (int i = 0; i < 100; i++)
                {
                    string line = new string('x', 100); // 100-character line
                    await writer.WriteLineAsync(line);
                    await writer.FlushAsync(); // Flush the buffer to send the data in chunks
                    await Task.Delay(10); // Wait for 1 second before writing the next line
                }
            }
        }
    }
}