using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v1
{
    [Produces("application/json")]
    [ApiController]
    [Route("v1/[controller]")]
    public class EmailController : ControllerBase
    {
        /// <inheritdoc />
        public EmailController() {}

        [HttpPost]
        public IActionResult ScheduleDailyEmail()
        {
            try
            {
                // Schedule the SendDailyEmailAsync method of EmailService to run daily
               RecurringJob.AddOrUpdate<EmailService>("SendDailyEmail", x => x.SendDailyEmailAsync(), Cron.Minutely);

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
        public async Task SendDailyEmailAsync()
        {
            // Asynchronous sending logic here
            await Task.Delay(1000); // Simulated asynchronous operation
        }
    }
}