using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Models.Hangfire;

namespace WebApi.Controllers.v1
{
    [Produces("application/json")]
    [ApiController]
    [Route("v1/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IBackgroundJobClient _backgroundJobClient;

        /// <inheritdoc />
        public EmailController(IBackgroundJobClient backgroundJobClient)
        {
            _backgroundJobClient = backgroundJobClient;
        }

        [HttpPost]
        public IActionResult ScheduleDailyEmail(HangfireRequest data)
        {
            try
            {
                // Schedule the SendDailyEmailAsync method of EmailService to run daily
               RecurringJob.AddOrUpdate<EmailService>("SendDailyEmail", x => x.SendDailyEmail(), Cron.Minutely);

                return Ok("Daily email job scheduled successfully");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }

    // Example EmailService class with a method to send daily emails
    public class EmailService
    {
        [AutomaticRetry(Attempts = 3)]
        public void SendDailyEmail()
        {
            //Send Logic
            Console.WriteLine("DONE");
        }
    }
}