using Microsoft.AspNetCore.Mvc;
using Models.Hangfire;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Services;

namespace WebApi.Controllers.v1
{
    /// <inheritdoc />
    [Produces("application/json")]
    [SwaggerTag("Requests for User Services")]
    [ApiController]
    [Route("v1/[controller]")]
    public class ReportsController(BctReportReminder recurring) : Controller
    {
        /// <summary>
        /// Create Example(s) Recurring job
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 401)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 500)]
        [HttpPost("reminder")]
        public IActionResult RecurringActionResult([FromBody] JobRequest data)
        {
            try
            {
                var test = recurring.CreateReminder(data.ClientId, data.TemplateId, data.Name, data.CronExpressionModel!);
                
                return !test.Item1 ? StatusCode(500, test.Item2) : Ok(test.Item2);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
