using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Linq;

namespace RespondWithStream
{
    public static class StreamWithLength
    {
        [FunctionName("RespondWithStreamWithLength")]
        public static async Task Run(
    [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "stream-text-with-length")] HttpRequest req,
    ILogger log)
        {
            try
            {
                List<string> list = new List<string>
        {
            "text1",
            "text2",
            "text3"
        };

                var response = req.HttpContext.Response;
                response.StatusCode = 200;
                response.ContentType = "application/json";
                response.Headers.Add("Access-Control-Allow-Origin", "*");
                response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
                response.Headers.Add("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept, photish-tokenkey");
                response.Headers.Add("Access-Control-Allow-Credentials", "true");

                int totalByteLength = list.Sum(x => Encoding.UTF8.GetBytes(x).Length);
                response.Headers.Add("Content-Length", totalByteLength.ToString());

                StringBuilder sb = new();
                await using var sw = new StreamWriter(response.Body);
                foreach (var item in list)
                {
                    sb.Append(item);
                    await sw.WriteAsync(item);
                    await sw.FlushAsync();

                    Thread.Sleep(2000);
                }
            }
            catch (Exception exc)
            {
                log.LogError(exc, "Issue when streaming: " + exc.Message);
            }
        }
    }
}
