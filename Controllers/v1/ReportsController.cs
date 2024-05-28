using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Models;
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
        public IActionResult RecurringActionResult([FromBody] BtcRptTemplateReminderRequest data)
        {
            try
            {
                var results = recurring.CreateReminder(data.Month, data.ReminderId, data.JobName, data.CronExpressionModel);

                return !results.Item1 ? StatusCode(500, results.Item2) : Ok(results.Item2);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete("reminder")]
        public IActionResult DeleteRecurringActionResult([FromQuery] string jobName)
        {
            try
            {
                var results = recurring.RemoveReminder(jobName);

                return !results.Item1 ? StatusCode(500, results.Item2) : Ok(results.Item2);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
