using LainTwitterBot.QuartzJobs;
using LainTwitterBot.Services;
using Quartz.Impl;
using Quartz;

namespace LainTwitterBot
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            StdSchedulerFactory factory = new StdSchedulerFactory();
            IScheduler scheduler = await factory.GetScheduler();

            await scheduler.Start();

            IJobDetail job = JobBuilder.Create<BotExecutionJob>()
                .WithIdentity("BotExecution", "Bot")
                .Build();


            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("DailyTrigger", "Bot")
                .StartNow()
                .WithSimpleSchedule(x => x
                    //.WithIntervalInSeconds(10)
                    .WithIntervalInHours(1)
                    .RepeatForever())
                .Build();

            await scheduler.ScheduleJob(job, trigger);
           
            Console.ReadKey();

            await scheduler.Shutdown();
        }
    }
}