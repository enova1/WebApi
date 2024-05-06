using Hangfire;

namespace WebApi.Services.Hangfire
{
    public class RunOnce
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="recipient"></param>
        /// <param name="body"></param>
        [AutomaticRetry(Attempts = 3)]
        public void SendEmail(string recipient, string body)
        {
            //Send Logic
            Console.WriteLine("DONE");
        }
    }
}
