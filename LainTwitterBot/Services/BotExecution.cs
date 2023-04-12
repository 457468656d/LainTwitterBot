using LainTwitterBot.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace LainTwitterBot.Services
{
    public class BotExecution : IBotExecution
    {
        private readonly TwitterClient _twitterClient;
        private readonly IImagesProvider _imagesProvider;
        private readonly ILogger<BotExecution> _logger;

        public BotExecution(TwitterClient twitterClient,
            IImagesProvider imagesProvider,
            ILogger<BotExecution> logger)
        {
            _twitterClient = twitterClient;
            _imagesProvider = imagesProvider;
            _logger = logger;
        }

        public async Task SendImageTweet()
        {
            try
            {
                var imageBytes = _imagesProvider.GetNextImageAsByteArray();

                _logger.Log(LogLevel.Information, "Uploading Image to Twittermedia...");

                var uploadedMedia = await _twitterClient.Upload.UploadBinaryAsync(imageBytes);
                _logger.Log(LogLevel.Information, "Successfully uploaded.");

                var tweetParams = new PublishTweetParameters
                {
                    Text = "#SerialExperimentsLain",
                    Medias = new List<IMedia> { uploadedMedia }
                };

                _logger.Log(LogLevel.Information, "Publishing tweet...");
                var tweet = await _twitterClient.Tweets.PublishTweetAsync(tweetParams);

                _logger.Log(LogLevel.Information, "Successfully published tweet.");
            }
            catch (Exception ex)
            {

                _logger.Log(LogLevel.Error, "Could not send tweet. Error:\n" +
                    ex.Message);
            }
        }
    }
}
