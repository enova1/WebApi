using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Models.Hangfire;
using Models.Hangfire.Enums;
using System.Collections.Generic;
using WebApi.Logic;

namespace WebApi.Controllers.v1
{
    /// <inheritdoc />
    [Produces("application/json")]
    [ApiController]
    [Route("v1/[controller]")]
    public class JobsController : Controller
    {

        /// <summary>
        /// Create Example(s) Recurring job
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("recurring")]
        public IActionResult RecurringActionResult(JobRequest data)
        {
                //TODO: Move to endpoints by JobScheduler type.
                //TODO: Add IF statement to check if CronExpression is null then use CronOption
            try
            {
                // Set the Cron by option = Minutely ("* * * * *")
                RecurringJob.AddOrUpdate<JobScheduler>($"{data.JobName}-Cron-Option-Minutely", x => x.Job1(13232, "test11"), 
                    $"{CronOptionEnum.Minutely.ToCronExpression()}");

                // Set the Cron by expression = Minutely ("* * * * *")
                RecurringJob.AddOrUpdate<JobScheduler>($"{data.JobName}-Cron-Expression-Minutely", x => x.Job2(436676, "test21"), 
                    $"{new CronExpressionModel(minute: "*", hour: "*", dayOfMonth: "*", month: "*")}");

                // Set the Cron by expression = Yearly ("0 0 19 12 * *")
                RecurringJob.AddOrUpdate<JobScheduler>($"{data.JobName}-Cron-Expression-OnceAYearOnDec19", x => x.Job3(data.ClientId, data.JobName+"(Yearly)", data.Email!), 
                    $"{new CronExpressionModel(minute: "0", hour: "0", dayOfMonth: "19", month: "12")}");

                return Ok($"Recurring job {data.JobName} has been scheduled successfully");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
