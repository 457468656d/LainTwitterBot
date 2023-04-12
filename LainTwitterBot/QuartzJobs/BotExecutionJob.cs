using LainTwitterBot.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LainTwitterBot.Services;
using Microsoft.Extensions.Configuration;
using Tweetinvi;
using Microsoft.Extensions.Logging;

namespace LainTwitterBot.QuartzJobs
{
    /// <summary>
    /// Class for setting up Botlogic as a Quartz.NET job.
    /// </summary>
    public class BotExecutionJob : IJob
    {
        private readonly IHost _host;
        public BotExecutionJob()
        {
            _host = Host.CreateDefaultBuilder()
                .UseSerilog((host, loggerConfiguration) =>
                {
                    loggerConfiguration.WriteTo.File("Logs.txt", rollingInterval: RollingInterval.Month);
                    loggerConfiguration.WriteTo.Console();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    string? consumerKey = hostContext.Configuration.GetValue<string>("ConsumerKey");
                    string? consumerSecret = hostContext.Configuration.GetValue<string>("ConsumerSecret");
                    string? accessToken = hostContext.Configuration.GetValue<string>("AccessToken");
                    string? accessTokenSecret = hostContext.Configuration.GetValue<string>("AccessTokenSecret");

                    services.AddSingleton((s) => new TwitterClient(consumerKey, consumerSecret, accessToken, accessTokenSecret));

                    services.AddSingleton<IBotExecution, BotExecution>();
                    services.AddSingleton<IImagesProvider>((s)=>
                        new ImagesProvider(
                            hostContext.Configuration.GetValue<string>("ImagesPath"),
                            s.GetRequiredService<ILogger<IImagesProvider>>()));

                })
                .Build();
        }
        public async Task Execute(IJobExecutionContext context)
        {
            await _host.StartAsync();
            try
            {
                var botExecution = _host.Services.GetRequiredService<IBotExecution>();
                await botExecution.SendImageTweet();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _host.Dispose();
            }
        }
    }
}
