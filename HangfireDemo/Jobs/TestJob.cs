namespace HangfireDemo.Jobs
{
    public class TestJob
    {
        private readonly ILogger _logger;

        public TestJob(ILogger<TestJob> logger)
        {
            _logger = logger;
        }

        public void WriteLog(string message) 
        { 
            _logger.LogInformation($"{DateTime.Now}{message}");
        }
    }
}
