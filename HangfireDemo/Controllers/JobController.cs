using Hangfire;
using HangfireDemo.Jobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HangfireDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        [HttpPost]
        [Route("CreateBackGroundJob")]
        public ActionResult CreateBackGroundJob()
        {
            BackgroundJob.Enqueue<TestJob>(x => x.WriteLog("Background job Triggered"));
            return Ok();
        }
        [HttpPost]
        [Route("CreateScheduledJob")]
        public ActionResult CreateScheduledJob()
        {
            var scheduledDateTime = DateTime.UtcNow.AddSeconds(5);
            var dateTimeOffSet = new DateTimeOffset(scheduledDateTime);
            BackgroundJob.Schedule<TestJob>(x => x.WriteLog("Scheduled job Triggered"), dateTimeOffSet);
            return Ok();
        }

        [HttpPost]
        [Route("CreateContinuationJob")]
        public ActionResult CreateContinuationJob()
        {
            var scheduledDateTime = DateTime.UtcNow.AddSeconds(5);
            var dateTimeOffSet = new DateTimeOffset(scheduledDateTime);
            var jobId = BackgroundJob.Schedule<TestJob>(x => x.WriteLog("Scheduled job 2 Triggered"), dateTimeOffSet);

            var job2Id = BackgroundJob.ContinueJobWith(jobId, () => Console.WriteLine("Continution Job 1 Triggered"));
            var job3Id = BackgroundJob.ContinueJobWith(job2Id, () => Console.WriteLine("Continution Job 2 Triggered"));
            var job4Id = BackgroundJob.ContinueJobWith(job3Id, () => Console.WriteLine("Continution Job 3 Triggered"));
            return Ok();
        }

        [HttpPost]
        [Route("CreateRecurringJob")]
        public ActionResult CreateRecurringJob()
        {
            RecurringJob.AddOrUpdate("RecurringJob1", () => Console.WriteLine("This job runs every minute"), "* * * * *");
            return Ok();
        }
    }
}
