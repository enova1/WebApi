using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Models.Hangfire;
using WebApi.Services;
using WebApi.Services.Hangfire;

namespace WebApi.Controllers.v1
{
    /// <inheritdoc />
    [Produces("application/json")]
    [ApiController]
    [Route("v1/[controller]")]
    public class JobsController(RunOnce runOnce, Delayed delayed, Continuations continuations) : ControllerBase
    {
        /// <summary>
        /// Create a Fire-and-Forget (Run Once) job
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult FireAndForgetActionResult(HangfireRequest data)
        {
            try
            {
                // Enqueue a background job to send an email
                BackgroundJob.Enqueue(() => runOnce.SendEmail(data.Email!, "Hello from BCT!"));

                return Ok("Fire-and-Forget job sent successfully");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Create a Delayed job
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult DelayedActionResult(HangfireRequest data)
        {
            try
            {
                // Schedule a background job to send a reminder after 1 hour
                BackgroundJob.Schedule(() => delayed.SendReminder(data.Email!, "Don't forget your appointment!"),
                    TimeSpan.FromHours(1));

                return Ok("Delayed job Scheduled successfully");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Create a NEW Recurring job
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult RecurringActionResult(HangfireRequest data)
        {
            try
            {
                // Schedule the Email to run daily (minutely for testing)
                RecurringJob.AddOrUpdate<Recurring>($"{data.JobName}", x => x.SendDailyEmail(), Cron.Minutely);

                return Ok($"Recurring job {data.JobName} has been scheduled successfully");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Create a Continuations job
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ContinuationsActionResult(HangfireRequest data)
        {
            try
            {
                // Enqueue a background job to process data
                var jobId = BackgroundJob.Enqueue(() => continuations.ProcessData(""));

                // Define a continuation job that runs after the data processing job completes
                BackgroundJob.ContinueJobWith(jobId, () => NotificationService.NotifyProcessingCompletion());


                return Ok($"Continuations job {data.JobName} has been scheduled successfully");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
