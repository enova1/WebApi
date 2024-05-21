using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebApi.Models;
using WebApi.Models.Enums;
using WebApi.Services;
using WebApi.Services.Helpers;

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
                //TODO: I hate this. I need to refactor this to use a factory pattern.
                string cron = data.CronExpressionModel != null
                    ? CronExpressionBuilder.Build(data.CronExpressionModel)
                    : CronExpressionBuilder.Build((CronOptionEnum)data.CronOption!);

                var results = recurring.CreateReminder(data.ClientId, data.TemplateId, data.Name, cron);

                return !results.Item1 ? StatusCode(500, results.Item2) : Ok(results.Item2);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
