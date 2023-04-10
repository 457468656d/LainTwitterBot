using LainTwitterBot.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LainTwitterBot.Services;
using Microsoft.Extensions.Configuration;
using Tweetinvi;

namespace LainTwitterBot.QuartzJobs
{
    public class BotExecutionJob : IJob
    {
        private readonly IHost _host;
        public BotExecutionJob()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    string consumerKey = hostContext.Configuration.GetValue<string>("ConsumerKey");
                    string consumerSecret = hostContext.Configuration.GetValue<string>("ConsumerSecret");
                    string accessToken = hostContext.Configuration.GetValue<string>("AccessToken");
                    string accessTokenSecret = hostContext.Configuration.GetValue<string>("AccessTokenSecret");

                    services.AddSingleton((s) => new TwitterClient(consumerKey, consumerSecret, accessToken, accessTokenSecret));

                    services.AddSingleton<IBotExecution, BotExecution>();
                    services.AddSingleton<IImagesProvider>(
                        new ImagesProvider(hostContext.Configuration.GetValue<string>("ImagesPath")));

                })
                .Build();
        }
        public async Task Execute(IJobExecutionContext context)
        {
            await _host.StartAsync();

            var botExecution = _host.Services.GetRequiredService<IBotExecution>();
            await botExecution.SendImageTweet();

            _host.Dispose();
        }
    }
}
